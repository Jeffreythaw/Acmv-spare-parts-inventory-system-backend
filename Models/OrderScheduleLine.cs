using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmvInventory.Models
{
    public class OrderScheduleLine
    {
        [Key]
        public int Id { get; set; }
        public string InventoryId { get; set; }
        public int Qty { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
    }
}
