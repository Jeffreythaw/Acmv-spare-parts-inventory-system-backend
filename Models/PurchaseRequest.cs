
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmvInventory.Models
{
    public class PurchaseRequest : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PRNo { get; set; }
        public string CreatedBy { get; set; }
        public PRStatus Status { get; set; } = PRStatus.DRAFT;
        public List<PRLine> Lines { get; set; } = new();
    }
}
