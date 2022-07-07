using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsUserRoleMembershipRoleValidOnlyJoined
    {
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("RoleID")]
        public int RoleId { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
