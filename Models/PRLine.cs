
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcmvInventory.Models
{
    public class PRLine
    {
        [Key]
        public int Id { get; set; }
        public string InventoryId { get; set; }
        public int RequestedQty { get; set; }
        public string SuggestedSupplierId { get; set; }
        public string Notes { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }
    }
}
