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
            if (schedule.Lines == null || schedule.Lines.Count == 0)
                return BadRequest(new { message = "Schedule must include at least one part line." });

            if (schedule.Lines.Any(l => l.Qty <= 0))
                return BadRequest(new { message = "Line quantity must be greater than 0." });

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
}
