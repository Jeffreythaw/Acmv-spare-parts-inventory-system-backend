
using AcmvInventory.Data;
using AcmvInventory.Models;
using Microsoft.EntityFrameworkCore;

namespace AcmvInventory.Services
{
    public class TransactionService
    {
        private readonly AcmvDbContext _context;

        public TransactionService(AcmvDbContext context)
        {
            _context = context;
        }

        public async Task<StockTransaction> CreateMovement(StockTransaction txn)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var line in txn.Lines)
                {
                    var inventory = await _context.Inventory
                        .FirstOrDefaultAsync(i => i.Id == line.InventoryId);

                    if (inventory == null) throw new Exception($"Item {line.InventoryId} not found");

                    line.BeforeQty = inventory.QuantityOnHand;

                    if (txn.TxnType == TxnType.ISSUE)
                    {
                        if (inventory.QuantityOnHand < line.Qty) 
                            throw new Exception($"Insufficient stock for {inventory.PartName}");
                        inventory.QuantityOnHand -= line.Qty;
                    }
                    else
                    {
                        inventory.QuantityOnHand += line.Qty;
                    }

                    line.AfterQty = inventory.QuantityOnHand;
                    inventory.LastUpdated = DateTime.UtcNow;
                }

                _context.Transactions.Add(txn);
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                
                return txn;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
