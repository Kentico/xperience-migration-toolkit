using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("OM_ContactGroupMember")]
    [Index("ContactGroupMemberContactGroupId", "ContactGroupMemberType", Name = "IX_OM_ContactGroupMember_ContactGroupID_Type_MemberID_RelatedID_FromCondition_FromAccount_FromManual")]
    [Index("ContactGroupMemberContactGroupId", "ContactGroupMemberType", "ContactGroupMemberRelatedId", Name = "IX_OM_ContactGroupMember_ContactGroupID_Type_RelatedID", IsUnique = true)]
    [Index("ContactGroupMemberRelatedId", Name = "IX_OM_ContactGroupMember_ContactGroupMemberRelatedID")]
    public partial class OmContactGroupMember
    {
        [Key]
        [Column("ContactGroupMemberID")]
        public int ContactGroupMemberId { get; set; }
        [Column("ContactGroupMemberContactGroupID")]
        public int ContactGroupMemberContactGroupId { get; set; }
        public int ContactGroupMemberType { get; set; }
        [Column("ContactGroupMemberRelatedID")]
        public int ContactGroupMemberRelatedId { get; set; }
        public bool? ContactGroupMemberFromCondition { get; set; }
        public bool? ContactGroupMemberFromAccount { get; set; }
        public bool? ContactGroupMemberFromManual { get; set; }

        [ForeignKey("ContactGroupMemberContactGroupId")]
        [InverseProperty("OmContactGroupMembers")]
        public virtual OmContactGroup ContactGroupMemberContactGroup { get; set; } = null!;
    }
}
