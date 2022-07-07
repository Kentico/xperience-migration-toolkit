using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_PageUrlPath")]
    [Index("PageUrlPathNodeId", Name = "IX_CMS_PageUrlPath_PageUrlPathNodeID")]
    [Index("PageUrlPathSiteId", Name = "IX_CMS_PageUrlPath_PageUrlPathSiteID")]
    [Index("PageUrlPathUrlPathHash", "PageUrlPathCulture", "PageUrlPathSiteId", Name = "IX_CMS_PageUrlPath_PageUrlPathUrlPathHash_PageUrlPathCulture_PageUrlPathSiteID", IsUnique = true)]
    public partial class CmsPageUrlPath
    {
        [Key]
        [Column("PageUrlPathID")]
        public int PageUrlPathId { get; set; }
        [Column("PageUrlPathGUID")]
        public Guid PageUrlPathGuid { get; set; }
        [StringLength(50)]
        public string PageUrlPathCulture { get; set; } = null!;
        [Column("PageUrlPathNodeID")]
        public int PageUrlPathNodeId { get; set; }
        [StringLength(2000)]
        public string PageUrlPathUrlPath { get; set; } = null!;
        [StringLength(64)]
        public string PageUrlPathUrlPathHash { get; set; } = null!;
        [Column("PageUrlPathSiteID")]
        public int PageUrlPathSiteId { get; set; }
        public DateTime PageUrlPathLastModified { get; set; }

        [ForeignKey("PageUrlPathNodeId")]
        [InverseProperty("CmsPageUrlPaths")]
        public virtual CmsTree PageUrlPathNode { get; set; } = null!;
        [ForeignKey("PageUrlPathSiteId")]
        [InverseProperty("CmsPageUrlPaths")]
        public virtual CmsSite PageUrlPathSite { get; set; } = null!;
    }
}
