
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AcmvDbContext _context;

        public DashboardController(AcmvDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var totalItems = await _context.Inventory.CountAsync();
            var lowStock = await _context.Inventory.CountAsync(i => i.QuantityOnHand <= (i.ReorderPoint ?? i.MinStock));
            var openPRs = await _context.PurchaseRequests.CountAsync(p => p.Status == PRStatus.SUBMITTED);
            var pendingPOs = await _context.PurchaseOrders.CountAsync(p => p.Status != POStatus.CLOSED && p.Status != POStatus.CANCELLED);

            return Ok(new
            {
                totalItems,
                lowStockCount = lowStock,
                openPRs,
                pendingPOs,
                avgStockoutDuration = "3.2 Days",
                onTimeDeliveryRate = 88
            });
        }
    }
}
