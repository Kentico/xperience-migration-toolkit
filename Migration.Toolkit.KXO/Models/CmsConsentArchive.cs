using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ConsentArchive")]
    [Index("ConsentArchiveConsentId", Name = "IX_ConsentArchive_ConsentArchiveConsentID")]
    public partial class CmsConsentArchive
    {
        [Key]
        [Column("ConsentArchiveID")]
        public int ConsentArchiveId { get; set; }
        public Guid ConsentArchiveGuid { get; set; }
        public DateTime ConsentArchiveLastModified { get; set; }
        [Column("ConsentArchiveConsentID")]
        public int ConsentArchiveConsentId { get; set; }
        [StringLength(100)]
        public string ConsentArchiveHash { get; set; } = null!;
        public string ConsentArchiveContent { get; set; } = null!;

        [ForeignKey("ConsentArchiveConsentId")]
        [InverseProperty("CmsConsentArchives")]
        public virtual CmsConsent ConsentArchiveConsent { get; set; } = null!;
    }
}
