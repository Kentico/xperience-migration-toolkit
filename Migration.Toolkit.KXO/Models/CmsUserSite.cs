using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_UserSite")]
    [Index("SiteId", Name = "IX_CMS_UserSite_SiteID")]
    [Index("UserId", "SiteId", Name = "IX_CMS_UserSite_UserID_SiteID", IsUnique = true)]
    public partial class CmsUserSite
    {
        [Key]
        [Column("UserSiteID")]
        public int UserSiteId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("SiteID")]
        public int SiteId { get; set; }

        [ForeignKey("SiteId")]
        [InverseProperty("CmsUserSites")]
        public virtual CmsSite Site { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CmsUserSites")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
