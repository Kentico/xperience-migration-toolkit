using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewMembershipMembershipUserJoined
    {
        [StringLength(200)]
        public string MembershipDisplayName { get; set; } = null!;
        [Column("MembershipID")]
        public int MembershipId { get; set; }
        public DateTime? ValidTo { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(254)]
        public string UserName { get; set; } = null!;
        [Column("MembershipSiteID")]
        public int? MembershipSiteId { get; set; }
    }
}
