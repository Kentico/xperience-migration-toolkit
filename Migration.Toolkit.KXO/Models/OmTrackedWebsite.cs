using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_TrackedWebsite")]
    [Index("TrackedWebsiteSiteId", Name = "IX_OM_TrackedWebsite_TrackedWebsiteSiteID")]
    public partial class OmTrackedWebsite
    {
        [Key]
        [Column("TrackedWebsiteID")]
        public int TrackedWebsiteId { get; set; }
        [Column("TrackedWebsiteGUID")]
        public Guid? TrackedWebsiteGuid { get; set; }
        [StringLength(200)]
        public string TrackedWebsiteDisplayName { get; set; } = null!;
        [StringLength(100)]
        public string TrackedWebsiteName { get; set; } = null!;
        [Column("TrackedWebsiteURL")]
        [StringLength(400)]
        public string TrackedWebsiteUrl { get; set; } = null!;
        public string? TrackedWebsiteDescription { get; set; }
        public bool TrackedWebsiteEnabled { get; set; }
        [Column("TrackedWebsiteSiteID")]
        public int TrackedWebsiteSiteId { get; set; }
        public DateTime? TrackedWebsiteLastModified { get; set; }

        [ForeignKey("TrackedWebsiteSiteId")]
        [InverseProperty("OmTrackedWebsites")]
        public virtual CmsSite TrackedWebsiteSite { get; set; } = null!;
    }
}
