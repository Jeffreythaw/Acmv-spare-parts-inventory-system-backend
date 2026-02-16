
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmvInventory.Models
{
    public class POLine
    {
        [Key]
        public int Id { get; set; }
        public string InventoryId { get; set; }
        public int OrderedQty { get; set; }
        public int ReceivedQty { get; set; }
        public decimal? UnitCost { get; set; }
        public DateTime? ETA { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
    }
}
