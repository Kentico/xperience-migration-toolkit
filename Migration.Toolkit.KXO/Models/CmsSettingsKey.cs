using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_SettingsKey")]
    [Index("KeyCategoryId", Name = "IX_CMS_SettingsKey_KeyCategoryID")]
    [Index("SiteId", "KeyName", Name = "IX_CMS_SettingsKey_SiteID_KeyName")]
    public partial class CmsSettingsKey
    {
        [Key]
        [Column("KeyID")]
        public int KeyId { get; set; }
        [StringLength(100)]
        public string KeyName { get; set; } = null!;
        [StringLength(200)]
        public string KeyDisplayName { get; set; } = null!;
        public string? KeyDescription { get; set; }
        public string? KeyValue { get; set; }
        [StringLength(50)]
        public string KeyType { get; set; } = null!;
        [Column("KeyCategoryID")]
        public int? KeyCategoryId { get; set; }
        [Column("SiteID")]
        public int? SiteId { get; set; }
        [Column("KeyGUID")]
        public Guid KeyGuid { get; set; }
        public DateTime KeyLastModified { get; set; }
        public int? KeyOrder { get; set; }
        public string? KeyDefaultValue { get; set; }
        [StringLength(255)]
        public string? KeyValidation { get; set; }
        [StringLength(200)]
        public string? KeyEditingControlPath { get; set; }
        public bool? KeyIsGlobal { get; set; }
        public bool? KeyIsCustom { get; set; }
        public bool? KeyIsHidden { get; set; }
        public string? KeyFormControlSettings { get; set; }
        public string? KeyExplanationText { get; set; }

        [ForeignKey("KeyCategoryId")]
        [InverseProperty("CmsSettingsKeys")]
        public virtual CmsSettingsCategory? KeyCategory { get; set; }
        [ForeignKey("SiteId")]
        [InverseProperty("CmsSettingsKeys")]
        public virtual CmsSite? Site { get; set; }
    }
}
