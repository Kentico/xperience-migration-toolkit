using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_UserCulture")]
    [Index("CultureId", Name = "IX_CMS_UserCulture_CultureID")]
    [Index("SiteId", Name = "IX_CMS_UserCulture_SiteID")]
    public partial class CmsUserCulture
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [Key]
        [Column("CultureID")]
        public int CultureId { get; set; }
        [Key]
        [Column("SiteID")]
        public int SiteId { get; set; }

        [ForeignKey("CultureId")]
        [InverseProperty("CmsUserCultures")]
        public virtual CmsCulture Culture { get; set; } = null!;
        [ForeignKey("SiteId")]
        [InverseProperty("CmsUserCultures")]
        public virtual CmsSite Site { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CmsUserCultures")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
