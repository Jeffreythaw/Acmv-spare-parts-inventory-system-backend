
using System;
using System.ComponentModel.DataAnnotations;

namespace AcmvInventory.Models
{
    public class Supplier : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
