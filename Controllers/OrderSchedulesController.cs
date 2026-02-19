using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderSchedulesController : ControllerBase
    {
        private readonly AcmvDbContext _context;

        public OrderSchedulesController(AcmvDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderSchedule>>> GetSchedules()
        {
            return await _context.OrderSchedules
                .Include(s => s.Lines)
                .OrderBy(s => s.ScheduledDate)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<OrderSchedule>> CreateSchedule(OrderSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid schedule payload.",
                    errors = ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage).ToArray()
                        )
                });
            }

            if (schedule == null)
                return BadRequest(new { message = "Schedule payload is required." });

            if (schedule.ScheduledDate == default)
                return BadRequest(new { message = "Scheduled date is required." });

            if (schedule.Lines == null || schedule.Lines.Count == 0)
                return BadRequest(new { message = "Schedule must include at least one part line." });

            if (schedule.Lines.Any(l => string.IsNullOrWhiteSpace(l.InventoryId)))
                return BadRequest(new { message = "Each line must include inventoryId." });

            if (schedule.Lines.Any(l => l.Qty <= 0))
                return BadRequest(new { message = "Line quantity must be greater than 0." });

            schedule.CreatedBy = string.IsNullOrWhiteSpace(schedule.CreatedBy) ? "System" : schedule.CreatedBy.Trim();
            schedule.SupplierId = schedule.SupplierId?.Trim() ?? string.Empty;
            schedule.Remark = schedule.Remark?.Trim() ?? string.Empty;
            schedule.CreatedAt = DateTime.UtcNow;
            schedule.LastUpdated = DateTime.UtcNow;
            _context.OrderSchedules.Add(schedule);
            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] ScheduleStatus status)
        {
            var schedule = await _context.OrderSchedules.FindAsync(id);
            if (schedule == null) return NotFound(new { message = "Schedule not found" });
            schedule.Status = status;
            schedule.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/reschedule")]
        public async Task<ActionResult<OrderSchedule>> Reschedule(string id, [FromBody] RescheduleRequest request)
        {
            if (request == null || request.ScheduledDate == default)
                return BadRequest(new { message = "Valid scheduledDate is required." });

            var schedule = await _context.OrderSchedules
                .Include(s => s.Lines)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (schedule == null) return NotFound(new { message = "Schedule not found" });

            if (schedule.Status == ScheduleStatus.CANCELLED || schedule.Status == ScheduleStatus.COMPLETED)
                return BadRequest(new { message = "Cannot postpone cancelled/completed schedule." });

            schedule.ScheduledDate = request.ScheduledDate;
            schedule.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        [HttpPost("{id}/receive")]
        public async Task<IActionResult> ReceiveSchedule(string id, [FromBody] List<ReceiveScheduleLineDto> lines)
        {
            if (lines == null || lines.Count == 0)
                return BadRequest(new { message = "At least one received line is required." });

            var schedule = await _context.OrderSchedules
                .Include(s => s.Lines)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
                return NotFound(new { message = "Schedule not found" });

            if (schedule.Status == ScheduleStatus.CANCELLED)
                return BadRequest(new { message = "Cannot receive cancelled schedule." });

            var inventoryIds = lines
                .Where(l => !string.IsNullOrWhiteSpace(l.InventoryId) && l.QtyReceived > 0)
                .Select(l => l.InventoryId)
                .Distinct()
                .ToList();

            if (inventoryIds.Count == 0)
                return BadRequest(new { message = "No valid received quantities found." });

            var inventoryMap = await _context.Inventory
                .Where(i => inventoryIds.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id, i => i);

            var txnLines = new List<TransactionLine>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.InventoryId) || line.QtyReceived <= 0)
                    continue;

                var scheduleLine = schedule.Lines.FirstOrDefault(l => l.InventoryId == line.InventoryId);
                if (scheduleLine == null)
                    return BadRequest(new { message = $"Part {line.InventoryId} is not in this schedule." });

                var pending = Math.Max(scheduleLine.Qty - scheduleLine.ReceivedQty, 0);
                if (line.QtyReceived > pending)
                    return BadRequest(new { message = $"Received qty for {line.InventoryId} exceeds outstanding ({pending})." });

                if (!inventoryMap.TryGetValue(line.InventoryId, out var inv))
                    return BadRequest(new { message = $"Inventory item {line.InventoryId} not found." });

                var before = inv.QuantityOnHand;
                var after = before + line.QtyReceived;

                inv.QuantityOnHand = after;
                inv.LastUpdated = DateTime.UtcNow;
                scheduleLine.ReceivedQty += line.QtyReceived;

                txnLines.Add(new TransactionLine
                {
                    InventoryId = line.InventoryId,
                    Qty = line.QtyReceived,
                    UnitCost = null,
                    BeforeQty = before,
                    AfterQty = after
                });
            }

            if (txnLines.Count == 0)
                return BadRequest(new { message = "No valid received quantities found." });

            var totalOutstanding = schedule.Lines.Sum(l => Math.Max(l.Qty - l.ReceivedQty, 0));
            if (totalOutstanding == 0 && schedule.Status != ScheduleStatus.CANCELLED)
                schedule.Status = ScheduleStatus.COMPLETED;

            schedule.LastUpdated = DateTime.UtcNow;

            _context.Transactions.Add(new StockTransaction
            {
                TxnType = TxnType.RECEIVE,
                TxnTime = DateTime.UtcNow,
                PerformedBy = "Storekeeper",
                Counterparty = string.IsNullOrWhiteSpace(schedule.SupplierId) ? "Schedule Receive" : schedule.SupplierId,
                Reference = schedule.Id,
                Remark = $"Receive from schedule {schedule.Id}",
                DocumentType = "ORDER_SCHEDULE",
                DocumentNo = schedule.Id,
                Lines = txnLines
            });

            await _context.SaveChangesAsync();
            return Ok(schedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var schedule = await _context.OrderSchedules
                .Include(s => s.Lines)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null) return NotFound(new { message = "Schedule not found" });
            _context.OrderScheduleLines.RemoveRange(schedule.Lines);
            _context.OrderSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class RescheduleRequest
    {
        public DateTime ScheduledDate { get; set; }
    }

    public class ReceiveScheduleLineDto
    {
        public string InventoryId { get; set; } = string.Empty;
        public int QtyReceived { get; set; }
    }
}
