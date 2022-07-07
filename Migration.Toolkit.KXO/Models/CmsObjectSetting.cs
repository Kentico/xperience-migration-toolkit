using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ObjectSettings")]
    [Index("ObjectCheckedOutByUserId", Name = "IX_CMS_ObjectSettings_ObjectCheckedOutByUserID")]
    [Index("ObjectCheckedOutVersionHistoryId", Name = "IX_CMS_ObjectSettings_ObjectCheckedOutVersionHistoryID")]
    [Index("ObjectPublishedVersionHistoryId", Name = "IX_CMS_ObjectSettings_ObjectPublishedVersionHistoryID")]
    [Index("ObjectSettingsObjectId", "ObjectSettingsObjectType", Name = "IX_CMS_ObjectSettings_ObjectSettingsObjectType_ObjectSettingsObjectID", IsUnique = true)]
    [Index("ObjectWorkflowStepId", Name = "IX_CMS_ObjectSettings_ObjectWorkflowStepID")]
    public partial class CmsObjectSetting
    {
        [Key]
        [Column("ObjectSettingsID")]
        public int ObjectSettingsId { get; set; }
        public string? ObjectTags { get; set; }
        [Column("ObjectCheckedOutByUserID")]
        public int? ObjectCheckedOutByUserId { get; set; }
        public DateTime? ObjectCheckedOutWhen { get; set; }
        [Column("ObjectCheckedOutVersionHistoryID")]
        public int? ObjectCheckedOutVersionHistoryId { get; set; }
        [Column("ObjectWorkflowStepID")]
        public int? ObjectWorkflowStepId { get; set; }
        [Column("ObjectPublishedVersionHistoryID")]
        public int? ObjectPublishedVersionHistoryId { get; set; }
        [Column("ObjectSettingsObjectID")]
        public int ObjectSettingsObjectId { get; set; }
        [StringLength(100)]
        public string ObjectSettingsObjectType { get; set; } = null!;
        public string? ObjectComments { get; set; }
        public bool? ObjectWorkflowSendEmails { get; set; }

        [ForeignKey("ObjectCheckedOutByUserId")]
        [InverseProperty("CmsObjectSettings")]
        public virtual CmsUser? ObjectCheckedOutByUser { get; set; }
        [ForeignKey("ObjectCheckedOutVersionHistoryId")]
        [InverseProperty("CmsObjectSettingObjectCheckedOutVersionHistories")]
        public virtual CmsObjectVersionHistory? ObjectCheckedOutVersionHistory { get; set; }
        [ForeignKey("ObjectPublishedVersionHistoryId")]
        [InverseProperty("CmsObjectSettingObjectPublishedVersionHistories")]
        public virtual CmsObjectVersionHistory? ObjectPublishedVersionHistory { get; set; }
        [ForeignKey("ObjectWorkflowStepId")]
        [InverseProperty("CmsObjectSettings")]
        public virtual CmsWorkflowStep? ObjectWorkflowStep { get; set; }
    }
}
