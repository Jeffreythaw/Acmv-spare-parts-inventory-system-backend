
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmvInventory.Models
{
    public class PurchaseOrder : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PONo { get; set; }
        public string SupplierId { get; set; }
        public string CreatedBy { get; set; }
        public POStatus Status { get; set; } = POStatus.DRAFT;
        public List<POLine> Lines { get; set; } = new();

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
