
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmvInventory.Models
{
    public class StockTransaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public TxnType TxnType { get; set; }
        public DateTime TxnTime { get; set; } = DateTime.UtcNow;
        public string PerformedBy { get; set; }
        public string Counterparty { get; set; }
        public string Reference { get; set; }
        public string Remark { get; set; }
        public List<TransactionLine> Lines { get; set; } = new();
    }
}
