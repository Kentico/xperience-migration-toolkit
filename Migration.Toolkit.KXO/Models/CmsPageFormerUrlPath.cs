using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_PageFormerUrlPath")]
    [Index("PageFormerUrlPathNodeId", Name = "IX_CMS_PageFormerUrlPath_PageFormerUrlPathNodeID")]
    [Index("PageFormerUrlPathSiteId", Name = "IX_CMS_PageFormerUrlPath_PageFormerUrlPathSiteID")]
    [Index("PageFormerUrlPathUrlPathHash", "PageFormerUrlPathCulture", "PageFormerUrlPathSiteId", Name = "IX_CMS_PageFormerUrlPath_UrlPathHash_Culture_SiteID", IsUnique = true)]
    public partial class CmsPageFormerUrlPath
    {
        [Key]
        [Column("PageFormerUrlPathID")]
        public int PageFormerUrlPathId { get; set; }
        [StringLength(2000)]
        public string PageFormerUrlPathUrlPath { get; set; } = null!;
        [StringLength(64)]
        public string PageFormerUrlPathUrlPathHash { get; set; } = null!;
        [StringLength(50)]
        public string PageFormerUrlPathCulture { get; set; } = null!;
        [Column("PageFormerUrlPathNodeID")]
        public int PageFormerUrlPathNodeId { get; set; }
        [Column("PageFormerUrlPathSiteID")]
        public int PageFormerUrlPathSiteId { get; set; }
        public DateTime PageFormerUrlPathLastModified { get; set; }

        [ForeignKey("PageFormerUrlPathNodeId")]
        [InverseProperty("CmsPageFormerUrlPaths")]
        public virtual CmsTree PageFormerUrlPathNode { get; set; } = null!;
        [ForeignKey("PageFormerUrlPathSiteId")]
        [InverseProperty("CmsPageFormerUrlPaths")]
        public virtual CmsSite PageFormerUrlPathSite { get; set; } = null!;
    }
}
