using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_SettingsCategory")]
    [Index("CategoryParentId", Name = "IX_CMS_SettingsCategory_CategoryParentID")]
    [Index("CategoryResourceId", Name = "IX_CMS_SettingsCategory_CategoryResourceID")]
    public partial class CmsSettingsCategory
    {
        public CmsSettingsCategory()
        {
            CmsSettingsKeys = new HashSet<CmsSettingsKey>();
            InverseCategoryParent = new HashSet<CmsSettingsCategory>();
        }

        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [StringLength(200)]
        public string CategoryDisplayName { get; set; } = null!;
        public int? CategoryOrder { get; set; }
        [StringLength(100)]
        public string? CategoryName { get; set; }
        [Column("CategoryParentID")]
        public int? CategoryParentId { get; set; }
        [Column("CategoryIDPath")]
        [StringLength(450)]
        public string? CategoryIdpath { get; set; }
        public int? CategoryLevel { get; set; }
        public int? CategoryChildCount { get; set; }
        [StringLength(200)]
        public string? CategoryIconPath { get; set; }
        public bool? CategoryIsGroup { get; set; }
        public bool? CategoryIsCustom { get; set; }
        [Column("CategoryResourceID")]
        public int? CategoryResourceId { get; set; }

        [ForeignKey("CategoryParentId")]
        [InverseProperty("InverseCategoryParent")]
        public virtual CmsSettingsCategory? CategoryParent { get; set; }
        [ForeignKey("CategoryResourceId")]
        [InverseProperty("CmsSettingsCategories")]
        public virtual CmsResource? CategoryResource { get; set; }
        [InverseProperty("KeyCategory")]
        public virtual ICollection<CmsSettingsKey> CmsSettingsKeys { get; set; }
        [InverseProperty("CategoryParent")]
        public virtual ICollection<CmsSettingsCategory> InverseCategoryParent { get; set; }
    }
}
