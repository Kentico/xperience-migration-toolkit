using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsRoleResourcePermissionJoined
    {
        [Column("RoleID")]
        public int RoleId { get; set; }
        [StringLength(100)]
        public string ResourceName { get; set; } = null!;
        [StringLength(100)]
        public string PermissionName { get; set; } = null!;
        [Column("PermissionID")]
        public int PermissionId { get; set; }
    }
}
