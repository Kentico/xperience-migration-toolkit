using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsUserRoleJoined
    {
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("RoleID")]
        public int RoleId { get; set; }
        public DateTime? ValidTo { get; set; }
        [StringLength(254)]
        public string UserName { get; set; } = null!;
        [Column("UserGUID")]
        public Guid UserGuid { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; } = null!;
        [StringLength(100)]
        public string RoleDisplayName { get; set; } = null!;
        [Column("RoleGUID")]
        public Guid RoleGuid { get; set; }
        [Column("SiteID")]
        public int? SiteId { get; set; }
        [StringLength(100)]
        public string? SiteName { get; set; }
        [Column("SiteGUID")]
        public Guid? SiteGuid { get; set; }
    }
}
