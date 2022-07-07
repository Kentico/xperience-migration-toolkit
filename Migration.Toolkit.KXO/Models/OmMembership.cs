using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_Membership")]
    [Index("ContactId", Name = "IX_OM_Membership_ContactID")]
    [Index("RelatedId", Name = "IX_OM_Membership_RelatedID")]
    public partial class OmMembership
    {
        [Key]
        [Column("MembershipID")]
        public int MembershipId { get; set; }
        [Column("RelatedID")]
        public int RelatedId { get; set; }
        public int MemberType { get; set; }
        [Column("MembershipGUID")]
        public Guid MembershipGuid { get; set; }
        public DateTime MembershipCreated { get; set; }
        [Column("ContactID")]
        public int ContactId { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("OmMemberships")]
        public virtual OmContact Contact { get; set; } = null!;
    }
}
