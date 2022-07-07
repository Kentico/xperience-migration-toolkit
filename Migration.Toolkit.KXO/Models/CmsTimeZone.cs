using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_TimeZone")]
    public partial class CmsTimeZone
    {
        [Key]
        [Column("TimeZoneID")]
        public int TimeZoneId { get; set; }
        [StringLength(200)]
        public string TimeZoneName { get; set; } = null!;
        [StringLength(200)]
        public string TimeZoneDisplayName { get; set; } = null!;
        [Column("TimeZoneGMT")]
        public double TimeZoneGmt { get; set; }
        public bool? TimeZoneDaylight { get; set; }
        public DateTime TimeZoneRuleStartIn { get; set; }
        [StringLength(200)]
        public string TimeZoneRuleStartRule { get; set; } = null!;
        public DateTime TimeZoneRuleEndIn { get; set; }
        [StringLength(200)]
        public string TimeZoneRuleEndRule { get; set; } = null!;
        [Column("TimeZoneGUID")]
        public Guid TimeZoneGuid { get; set; }
        public DateTime TimeZoneLastModified { get; set; }
    }
}
