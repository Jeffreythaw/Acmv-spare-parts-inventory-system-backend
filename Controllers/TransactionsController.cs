
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Models;
using AcmvInventory.Services;

namespace AcmvInventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AcmvDbContext _context;
        private readonly TransactionService _transactionService;

        public TransactionsController(AcmvDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockTransaction>>> GetTransactions()
        {
            return await _context.Transactions
                .Include(t => t.Lines)
                .OrderByDescending(t => t.TxnTime)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<StockTransaction>> CreateTransaction(StockTransaction txn)
        {
            try
            {
                var result = await _transactionService.CreateMovement(txn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StockTransaction>> UpdateTransaction(string id, StockTransaction txn)
        {
            if (id != txn.Id) return BadRequest(new { message = "ID mismatch" });

            var existing = await _context.Transactions
                .Include(t => t.Lines)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existing == null) return NotFound(new { message = "Transaction not found" });

            // Revert existing effect first.
            foreach (var line in existing.Lines)
            {
                var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.Id == line.InventoryId);
                if (inventory == null) continue;
                var revertDelta = existing.TxnType == TxnType.ISSUE ? line.Qty : -line.Qty;
                inventory.QuantityOnHand += revertDelta;
                inventory.LastUpdated = DateTime.UtcNow;
            }

            // Remove old lines and apply new transaction values.
            _context.TransactionLines.RemoveRange(existing.Lines);
            existing.TxnType = txn.TxnType;
            existing.Counterparty = txn.Counterparty;
            existing.PerformedBy = txn.PerformedBy;
            existing.Reference = txn.Reference;
            existing.Remark = txn.Remark;
            existing.Lines = txn.Lines ?? new List<TransactionLine>();

            foreach (var line in existing.Lines)
            {
                var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.Id == line.InventoryId);
                if (inventory == null) return BadRequest(new { message = $"Item {line.InventoryId} not found" });

                line.BeforeQty = inventory.QuantityOnHand;
                var delta = existing.TxnType == TxnType.ISSUE ? -line.Qty : line.Qty;
                if (delta < 0 && Math.Abs(delta) > inventory.QuantityOnHand)
                    return BadRequest(new { message = $"Insufficient stock for {inventory.PartName}" });

                inventory.QuantityOnHand += delta;
                inventory.LastUpdated = DateTime.UtcNow;
                line.AfterQty = inventory.QuantityOnHand;
            }

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            var txn = await _context.Transactions
                .Include(t => t.Lines)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (txn == null) return NotFound(new { message = "Transaction not found" });

            // Revert stock effect.
            foreach (var line in txn.Lines)
            {
                var inventory = await _context.Inventory.FirstOrDefaultAsync(i => i.Id == line.InventoryId);
                if (inventory == null) continue;
                var revertDelta = txn.TxnType == TxnType.ISSUE ? line.Qty : -line.Qty;
                inventory.QuantityOnHand += revertDelta;
                inventory.LastUpdated = DateTime.UtcNow;
            }

            _context.TransactionLines.RemoveRange(txn.Lines);
            _context.Transactions.Remove(txn);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
