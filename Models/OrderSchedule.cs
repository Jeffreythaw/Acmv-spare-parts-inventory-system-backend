using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmvInventory.Models
{
    public class OrderSchedule : BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime ScheduledDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public ScheduleStatus Status { get; set; } = ScheduleStatus.SCHEDULED;
        public List<OrderScheduleLine> Lines { get; set; } = new();
    }
}
