
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmvInventory.Models
{
    public class TransactionLine
    {
        [Key]
        public int Id { get; set; }
        public string InventoryId { get; set; }
        public int Qty { get; set; }
        public decimal? UnitCost { get; set; }
        public int BeforeQty { get; set; }
        public int AfterQty { get; set; }
        
        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
    }
}
