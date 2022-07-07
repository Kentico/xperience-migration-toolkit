using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_MembershipUser")]
    [Index("MembershipId", "UserId", Name = "IX_CMS_MembershipUser_MembershipID_UserID", IsUnique = true)]
    [Index("UserId", Name = "IX_CMS_MembershipUser_UserID")]
    public partial class CmsMembershipUser
    {
        [Key]
        [Column("MembershipUserID")]
        public int MembershipUserId { get; set; }
        [Column("MembershipID")]
        public int MembershipId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        public DateTime? ValidTo { get; set; }
        public bool? SendNotification { get; set; }

        [ForeignKey("MembershipId")]
        [InverseProperty("CmsMembershipUsers")]
        public virtual CmsMembership Membership { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CmsMembershipUsers")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
