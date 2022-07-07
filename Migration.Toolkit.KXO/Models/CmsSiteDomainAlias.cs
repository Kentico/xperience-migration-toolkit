using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_SiteDomainAlias")]
    [Index("SiteDomainAliasName", Name = "IX_CMS_SiteDomainAlias_SiteDomainAliasName")]
    [Index("SiteId", Name = "IX_CMS_SiteDomainAlias_SiteID")]
    public partial class CmsSiteDomainAlias
    {
        [Key]
        [Column("SiteDomainAliasID")]
        public int SiteDomainAliasId { get; set; }
        [StringLength(400)]
        public string? SiteDomainAliasName { get; set; }
        [Column("SiteID")]
        public int SiteId { get; set; }
        [StringLength(50)]
        public string? SiteDefaultVisitorCulture { get; set; }
        [Column("SiteDomainGUID")]
        public Guid SiteDomainGuid { get; set; }
        public DateTime SiteDomainLastModified { get; set; }

        [ForeignKey("SiteId")]
        [InverseProperty("CmsSiteDomainAliases")]
        public virtual CmsSite Site { get; set; } = null!;
    }
}
