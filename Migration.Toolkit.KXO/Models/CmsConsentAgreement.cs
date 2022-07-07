using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ConsentAgreement")]
    [Index("ConsentAgreementContactId", "ConsentAgreementConsentId", Name = "IX_CMS_ConsentAgreement_ConsentAgreementContactID_ConsentAgreementConsentID")]
    public partial class CmsConsentAgreement
    {
        [Key]
        [Column("ConsentAgreementID")]
        public int ConsentAgreementId { get; set; }
        public Guid ConsentAgreementGuid { get; set; }
        public bool ConsentAgreementRevoked { get; set; }
        [Column("ConsentAgreementContactID")]
        public int ConsentAgreementContactId { get; set; }
        [Column("ConsentAgreementConsentID")]
        public int ConsentAgreementConsentId { get; set; }
        [StringLength(100)]
        public string? ConsentAgreementConsentHash { get; set; }
        public DateTime ConsentAgreementTime { get; set; }

        [ForeignKey("ConsentAgreementConsentId")]
        [InverseProperty("CmsConsentAgreements")]
        public virtual CmsConsent ConsentAgreementConsent { get; set; } = null!;
        [ForeignKey("ConsentAgreementContactId")]
        [InverseProperty("CmsConsentAgreements")]
        public virtual OmContact ConsentAgreementContact { get; set; } = null!;
    }
}
