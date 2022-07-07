using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_EventLog")]
    [Index("SiteId", Name = "IX_CMS_EventLog_SiteID")]
    public partial class CmsEventLog
    {
        [Key]
        [Column("EventID")]
        public int EventId { get; set; }
        [StringLength(5)]
        public string EventType { get; set; } = null!;
        public DateTime EventTime { get; set; }
        [StringLength(100)]
        public string Source { get; set; } = null!;
        [StringLength(100)]
        public string EventCode { get; set; } = null!;
        [Column("UserID")]
        public int? UserId { get; set; }
        [StringLength(250)]
        public string? UserName { get; set; }
        [Column("IPAddress")]
        [StringLength(100)]
        public string? Ipaddress { get; set; }
        [Column("NodeID")]
        public int? NodeId { get; set; }
        [StringLength(100)]
        public string? DocumentName { get; set; }
        public string? EventDescription { get; set; }
        [Column("SiteID")]
        public int? SiteId { get; set; }
        public string? EventUrl { get; set; }
        [StringLength(100)]
        public string? EventMachineName { get; set; }
        public string? EventUserAgent { get; set; }
        public string? EventUrlReferrer { get; set; }
    }
}
