
using System;
using System.ComponentModel.DataAnnotations;

namespace AcmvInventory.Models
{
    public class Inventory : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Building { get; set; }
        public string Room { get; set; }
        public string TagNo { get; set; }
        public string InstallationType { get; set; }
        public string SystemType { get; set; }
        public string Brand { get; set; }
        public string EquipmentModel { get; set; }
        public string PartCategory { get; set; }
        [Required]
        public string PartName { get; set; }
        public string PartModel { get; set; }
        public string Unit { get; set; } = "pcs";
        public PartStatus Status { get; set; }
        public Criticality Criticality { get; set; }
        public string ImageBase64 { get; set; }
        public string Specs { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public string Remark { get; set; }
        public int QuantityOnHand { get; set; }
        public int MinStock { get; set; }
        public int? ReorderPoint { get; set; }
        public int? ReorderQty { get; set; }
        public string PreferredSupplierId { get; set; }
        public string LocationBin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
