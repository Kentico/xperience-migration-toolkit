using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_AccountContact")]
    [Index("AccountId", Name = "IX_OM_AccountContact_AccountID")]
    [Index("ContactId", Name = "IX_OM_AccountContact_ContactID")]
    [Index("ContactRoleId", Name = "IX_OM_AccountContact_ContactRoleID")]
    public partial class OmAccountContact
    {
        [Key]
        [Column("AccountContactID")]
        public int AccountContactId { get; set; }
        [Column("ContactRoleID")]
        public int? ContactRoleId { get; set; }
        [Column("AccountID")]
        public int AccountId { get; set; }
        [Column("ContactID")]
        public int ContactId { get; set; }

        [ForeignKey("AccountId")]
        [InverseProperty("OmAccountContacts")]
        public virtual OmAccount Account { get; set; } = null!;
        [ForeignKey("ContactId")]
        [InverseProperty("OmAccountContacts")]
        public virtual OmContact Contact { get; set; } = null!;
        [ForeignKey("ContactRoleId")]
        [InverseProperty("OmAccountContacts")]
        public virtual OmContactRole? ContactRole { get; set; }
    }
}
