
using AcmvInventory.Data;
using AcmvInventory.Models;
using AcmvInventory.Controllers;
using Microsoft.EntityFrameworkCore;

namespace AcmvInventory.Services
{
    public class PurchasingService
    {
        private readonly AcmvDbContext _context;
        private readonly TransactionService _transactionService;

        public PurchasingService(AcmvDbContext context, TransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        public async Task<PurchaseOrder> GeneratePOFromPR(string prId)
        {
            var pr = await _context.PurchaseRequests.Include(p => p.Lines).FirstOrDefaultAsync(p => p.Id == prId);
            if (pr == null || pr.Status != PRStatus.APPROVED) 
                throw new Exception("Only approved PRs can be converted to POs.");

            var po = new PurchaseOrder
            {
                PONo = $"PO-{DateTime.Now.Ticks.ToString().Substring(10)}",
                SupplierId = pr.Lines.FirstOrDefault()?.SuggestedSupplierId ?? "default_supplier",
                CreatedBy = pr.CreatedBy,
                Status = POStatus.DRAFT,
                Lines = pr.Lines.Select(l => new POLine {
                    InventoryId = l.InventoryId,
                    OrderedQty = l.RequestedQty,
                    ReceivedQty = 0
                }).ToList()
            };

            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();
            return po;
        }

        public async Task ProcessPOReceipt(string poId, List<ReceiveLineDto> receiptLines)
        {
            var po = await _context.PurchaseOrders.Include(p => p.Lines).FirstOrDefaultAsync(p => p.Id == poId);
            if (po == null) throw new Exception("PO not found");

            var stockTxn = new StockTransaction
            {
                TxnType = TxnType.RECEIVE,
                PerformedBy = "System Processor",
                Counterparty = "Vendor",
                Reference = po.PONo,
                Remark = "PO Receiving Process",
                Lines = new List<TransactionLine>()
            };

            foreach (var rLine in receiptLines)
            {
                var poLine = po.Lines.FirstOrDefault(l => l.InventoryId == rLine.InventoryId);
                if (poLine != null)
                {
                    poLine.ReceivedQty += rLine.QtyReceived;
                    poLine.UnitCost = rLine.UnitCost;
                    
                    stockTxn.Lines.Add(new TransactionLine {
                        InventoryId = rLine.InventoryId,
                        Qty = rLine.QtyReceived,
                        UnitCost = rLine.UnitCost
                    });
                }
            }

            // Update PO status
            bool allReceived = po.Lines.All(l => l.ReceivedQty >= l.OrderedQty);
            po.Status = allReceived ? POStatus.CLOSED : POStatus.PARTIALLY_RECEIVED;

            await _transactionService.CreateMovement(stockTxn);
            await _context.SaveChangesAsync();
        }
    }
}
