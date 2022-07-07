using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ObjectVersionHistory")]
    [Index("VersionDeletedByUserId", "VersionDeletedWhen", Name = "IX_CMS_ObjectVersionHistory_VersionDeletedByUserID_VersionDeletedWhen")]
    [Index("VersionModifiedByUserId", Name = "IX_CMS_ObjectVersionHistory_VersionModifiedByUserID")]
    [Index("VersionObjectSiteId", "VersionDeletedWhen", Name = "IX_CMS_ObjectVersionHistory_VersionObjectSiteID_VersionDeletedWhen")]
    [Index("VersionObjectType", "VersionObjectId", "VersionModifiedWhen", Name = "IX_CMS_ObjectVersionHistory_VersionObjectType_VersionObjectID_VersionModifiedWhen")]
    public partial class CmsObjectVersionHistory
    {
        public CmsObjectVersionHistory()
        {
            CmsObjectSettingObjectCheckedOutVersionHistories = new HashSet<CmsObjectSetting>();
            CmsObjectSettingObjectPublishedVersionHistories = new HashSet<CmsObjectSetting>();
        }

        [Key]
        [Column("VersionID")]
        public int VersionId { get; set; }
        [Column("VersionObjectID")]
        public int? VersionObjectId { get; set; }
        [StringLength(100)]
        public string VersionObjectType { get; set; } = null!;
        [Column("VersionObjectSiteID")]
        public int? VersionObjectSiteId { get; set; }
        [StringLength(450)]
        public string VersionObjectDisplayName { get; set; } = null!;
        [Column("VersionXML")]
        public string VersionXml { get; set; } = null!;
        [Column("VersionBinaryDataXML")]
        public string? VersionBinaryDataXml { get; set; }
        [Column("VersionModifiedByUserID")]
        public int? VersionModifiedByUserId { get; set; }
        public DateTime VersionModifiedWhen { get; set; }
        [Column("VersionDeletedByUserID")]
        public int? VersionDeletedByUserId { get; set; }
        public DateTime? VersionDeletedWhen { get; set; }
        [StringLength(50)]
        public string VersionNumber { get; set; } = null!;
        [Column("VersionSiteBindingIDs")]
        public string? VersionSiteBindingIds { get; set; }
        public string? VersionComment { get; set; }

        [ForeignKey("VersionDeletedByUserId")]
        [InverseProperty("CmsObjectVersionHistoryVersionDeletedByUsers")]
        public virtual CmsUser? VersionDeletedByUser { get; set; }
        [ForeignKey("VersionModifiedByUserId")]
        [InverseProperty("CmsObjectVersionHistoryVersionModifiedByUsers")]
        public virtual CmsUser? VersionModifiedByUser { get; set; }
        [ForeignKey("VersionObjectSiteId")]
        [InverseProperty("CmsObjectVersionHistories")]
        public virtual CmsSite? VersionObjectSite { get; set; }
        [InverseProperty("ObjectCheckedOutVersionHistory")]
        public virtual ICollection<CmsObjectSetting> CmsObjectSettingObjectCheckedOutVersionHistories { get; set; }
        [InverseProperty("ObjectPublishedVersionHistory")]
        public virtual ICollection<CmsObjectSetting> CmsObjectSettingObjectPublishedVersionHistories { get; set; }
    }
}
