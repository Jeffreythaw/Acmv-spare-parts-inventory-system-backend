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
        public string CreatedBy { get; set; }
        public string SupplierId { get; set; }
        public string Remark { get; set; }
        public ScheduleStatus Status { get; set; } = ScheduleStatus.SCHEDULED;
        public List<OrderScheduleLine> Lines { get; set; } = new();
    }
}
