using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Membership")]
    [Index("MembershipSiteId", Name = "IX_CMS_Membership_MembershipSiteID")]
    public partial class CmsMembership
    {
        public CmsMembership()
        {
            CmsMembershipUsers = new HashSet<CmsMembershipUser>();
            Roles = new HashSet<CmsRole>();
        }

        [Key]
        [Column("MembershipID")]
        public int MembershipId { get; set; }
        [StringLength(200)]
        public string MembershipName { get; set; } = null!;
        [StringLength(200)]
        public string MembershipDisplayName { get; set; } = null!;
        public string? MembershipDescription { get; set; }
        public DateTime MembershipLastModified { get; set; }
        [Column("MembershipGUID")]
        public Guid MembershipGuid { get; set; }
        [Column("MembershipSiteID")]
        public int? MembershipSiteId { get; set; }

        [ForeignKey("MembershipSiteId")]
        [InverseProperty("CmsMemberships")]
        public virtual CmsSite? MembershipSite { get; set; }
        [InverseProperty("Membership")]
        public virtual ICollection<CmsMembershipUser> CmsMembershipUsers { get; set; }

        [ForeignKey("MembershipId")]
        [InverseProperty("Memberships")]
        public virtual ICollection<CmsRole> Roles { get; set; }
    }
}
