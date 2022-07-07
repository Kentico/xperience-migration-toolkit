using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_Account")]
    [Index("AccountCountryId", Name = "IX_OM_Account_AccountCountryID")]
    [Index("AccountOwnerUserId", Name = "IX_OM_Account_AccountOwnerUserID")]
    [Index("AccountPrimaryContactId", Name = "IX_OM_Account_AccountPrimaryContactID")]
    [Index("AccountSecondaryContactId", Name = "IX_OM_Account_AccountSecondaryContactID")]
    [Index("AccountStateId", Name = "IX_OM_Account_AccountStateID")]
    [Index("AccountStatusId", Name = "IX_OM_Account_AccountStatusID")]
    [Index("AccountSubsidiaryOfId", Name = "IX_OM_Account_AccountSubsidiaryOfID")]
    public partial class OmAccount
    {
        public OmAccount()
        {
            InverseAccountSubsidiaryOf = new HashSet<OmAccount>();
            OmAccountContacts = new HashSet<OmAccountContact>();
        }

        [Key]
        [Column("AccountID")]
        public int AccountId { get; set; }
        [StringLength(200)]
        public string AccountName { get; set; } = null!;
        [StringLength(100)]
        public string? AccountAddress1 { get; set; }
        [StringLength(100)]
        public string? AccountAddress2 { get; set; }
        [StringLength(100)]
        public string? AccountCity { get; set; }
        [Column("AccountZIP")]
        [StringLength(20)]
        public string? AccountZip { get; set; }
        [Column("AccountStateID")]
        public int? AccountStateId { get; set; }
        [Column("AccountCountryID")]
        public int? AccountCountryId { get; set; }
        [StringLength(200)]
        public string? AccountWebSite { get; set; }
        [StringLength(26)]
        public string? AccountPhone { get; set; }
        [StringLength(254)]
        public string? AccountEmail { get; set; }
        [StringLength(26)]
        public string? AccountFax { get; set; }
        [Column("AccountPrimaryContactID")]
        public int? AccountPrimaryContactId { get; set; }
        [Column("AccountSecondaryContactID")]
        public int? AccountSecondaryContactId { get; set; }
        [Column("AccountStatusID")]
        public int? AccountStatusId { get; set; }
        public string? AccountNotes { get; set; }
        [Column("AccountOwnerUserID")]
        public int? AccountOwnerUserId { get; set; }
        [Column("AccountSubsidiaryOfID")]
        public int? AccountSubsidiaryOfId { get; set; }
        [Column("AccountGUID")]
        public Guid AccountGuid { get; set; }
        public DateTime AccountLastModified { get; set; }
        public DateTime AccountCreated { get; set; }

        [ForeignKey("AccountCountryId")]
        [InverseProperty("OmAccounts")]
        public virtual CmsCountry? AccountCountry { get; set; }
        [ForeignKey("AccountOwnerUserId")]
        [InverseProperty("OmAccounts")]
        public virtual CmsUser? AccountOwnerUser { get; set; }
        [ForeignKey("AccountPrimaryContactId")]
        [InverseProperty("OmAccountAccountPrimaryContacts")]
        public virtual OmContact? AccountPrimaryContact { get; set; }
        [ForeignKey("AccountSecondaryContactId")]
        [InverseProperty("OmAccountAccountSecondaryContacts")]
        public virtual OmContact? AccountSecondaryContact { get; set; }
        [ForeignKey("AccountStateId")]
        [InverseProperty("OmAccounts")]
        public virtual CmsState? AccountState { get; set; }
        [ForeignKey("AccountStatusId")]
        [InverseProperty("OmAccounts")]
        public virtual OmAccountStatus? AccountStatus { get; set; }
        [ForeignKey("AccountSubsidiaryOfId")]
        [InverseProperty("InverseAccountSubsidiaryOf")]
        public virtual OmAccount? AccountSubsidiaryOf { get; set; }
        [InverseProperty("AccountSubsidiaryOf")]
        public virtual ICollection<OmAccount> InverseAccountSubsidiaryOf { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<OmAccountContact> OmAccountContacts { get; set; }
    }
}
