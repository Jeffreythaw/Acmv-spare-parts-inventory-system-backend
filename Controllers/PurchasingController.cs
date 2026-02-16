
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;
using AcmvInventory.Services;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasingController : ControllerBase
    {
        private readonly AcmvDbContext _context;
        private readonly PurchasingService _purchasingService;

        public PurchasingController(AcmvDbContext context, PurchasingService purchasingService)
        {
            _context = context;
            _purchasingService = purchasingService;
        }

        // Purchase Requests
        [HttpGet("pr")]
        public async Task<ActionResult<IEnumerable<PurchaseRequest>>> GetPRs() => 
            await _context.PurchaseRequests.Include(p => p.Lines).ToListAsync();

        [HttpPost("pr")]
        public async Task<ActionResult<PurchaseRequest>> CreatePR(PurchaseRequest pr)
        {
            pr.PRNo = $"PR-{DateTime.Now.Ticks.ToString().Substring(10)}";
            _context.PurchaseRequests.Add(pr);
            await _context.SaveChangesAsync();
            return Ok(pr);
        }

        [HttpPost("pr/{id}/approve")]
        public async Task<IActionResult> ApprovePR(string id)
        {
            var pr = await _context.PurchaseRequests.FindAsync(id);
            if (pr == null) return NotFound();
            pr.Status = PRStatus.APPROVED;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("pr/{id}/convert-to-po")]
        public async Task<ActionResult<PurchaseOrder>> ConvertToPO(string id)
        {
            try {
                var po = await _purchasingService.GeneratePOFromPR(id);
                return Ok(po);
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Purchase Orders
        [HttpGet("po")]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPOs() => 
            await _context.PurchaseOrders.Include(p => p.Lines).Include(p => p.Supplier).ToListAsync();

        [HttpPost("po/{id}/receive")]
        public async Task<IActionResult> ReceivePO(string id, [FromBody] List<ReceiveLineDto> lines)
        {
            try {
                await _purchasingService.ProcessPOReceipt(id, lines);
                return Ok();
            } catch (Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class ReceiveLineDto {
        public string InventoryId { get; set; }
        public int QtyReceived { get; set; }
        public decimal? UnitCost { get; set; }
    }
}
