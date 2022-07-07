using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsSiteDocumentCount
    {
        [Column("SiteID")]
        public int SiteId { get; set; }
        [StringLength(100)]
        public string SiteName { get; set; } = null!;
        [StringLength(200)]
        public string SiteDisplayName { get; set; } = null!;
        public string? SiteDescription { get; set; }
        [StringLength(20)]
        public string SiteStatus { get; set; } = null!;
        [StringLength(400)]
        public string SiteDomainName { get; set; } = null!;
        [StringLength(50)]
        public string? SiteDefaultVisitorCulture { get; set; }
        [Column("SiteGUID")]
        public Guid SiteGuid { get; set; }
        public DateTime SiteLastModified { get; set; }
        public int? Documents { get; set; }
    }
}
