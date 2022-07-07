using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Consent")]
    public partial class CmsConsent
    {
        public CmsConsent()
        {
            CmsConsentAgreements = new HashSet<CmsConsentAgreement>();
            CmsConsentArchives = new HashSet<CmsConsentArchive>();
        }

        [Key]
        [Column("ConsentID")]
        public int ConsentId { get; set; }
        [StringLength(200)]
        public string ConsentDisplayName { get; set; } = null!;
        [StringLength(200)]
        public string ConsentName { get; set; } = null!;
        public string ConsentContent { get; set; } = null!;
        public Guid ConsentGuid { get; set; }
        public DateTime ConsentLastModified { get; set; }
        [StringLength(100)]
        public string ConsentHash { get; set; } = null!;

        [InverseProperty("ConsentAgreementConsent")]
        public virtual ICollection<CmsConsentAgreement> CmsConsentAgreements { get; set; }
        [InverseProperty("ConsentArchiveConsent")]
        public virtual ICollection<CmsConsentArchive> CmsConsentArchives { get; set; }
    }
}
