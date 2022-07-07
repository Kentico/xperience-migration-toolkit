using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_Activity")]
    [Index("ActivityContactId", Name = "IX_OM_Activity_ActivityContactID")]
    [Index("ActivityCreated", Name = "IX_OM_Activity_ActivityCreated")]
    [Index("ActivityItemDetailId", Name = "IX_OM_Activity_ActivityItemDetailID")]
    [Index("ActivitySiteId", Name = "IX_OM_Activity_ActivitySiteID")]
    [Index("ActivityType", "ActivityItemId", "ActivityNodeId", Name = "IX_OM_Activity_ActivityType_ActivityItemID_ActivityNodeID_ActivityUTMSource_ActivityUTMContent_ActivityCampaign")]
    public partial class OmActivity
    {
        [Key]
        [Column("ActivityID")]
        public int ActivityId { get; set; }
        [Column("ActivityContactID")]
        public int ActivityContactId { get; set; }
        public DateTime? ActivityCreated { get; set; }
        [StringLength(250)]
        public string ActivityType { get; set; } = null!;
        [Column("ActivityItemID")]
        public int? ActivityItemId { get; set; }
        [Column("ActivityItemDetailID")]
        public int? ActivityItemDetailId { get; set; }
        [StringLength(250)]
        public string? ActivityValue { get; set; }
        [Column("ActivityURL")]
        public string? ActivityUrl { get; set; }
        [StringLength(250)]
        public string? ActivityTitle { get; set; }
        [Column("ActivitySiteID")]
        public int ActivitySiteId { get; set; }
        public string? ActivityComment { get; set; }
        [StringLength(200)]
        public string? ActivityCampaign { get; set; }
        [Column("ActivityURLReferrer")]
        public string? ActivityUrlreferrer { get; set; }
        [StringLength(50)]
        public string? ActivityCulture { get; set; }
        [Column("ActivityNodeID")]
        public int? ActivityNodeId { get; set; }
        [Column("ActivityUTMSource")]
        [StringLength(200)]
        public string? ActivityUtmsource { get; set; }
        [Column("ActivityABVariantName")]
        [StringLength(200)]
        public string? ActivityAbvariantName { get; set; }
        [Column("ActivityURLHash")]
        public long ActivityUrlhash { get; set; }
        [Column("ActivityUTMContent")]
        [StringLength(200)]
        public string? ActivityUtmcontent { get; set; }
        [Column("ActivityTrackedWebsiteID")]
        public int? ActivityTrackedWebsiteId { get; set; }
    }
}
