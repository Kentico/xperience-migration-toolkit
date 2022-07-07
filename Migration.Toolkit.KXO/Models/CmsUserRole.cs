using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_UserRole")]
    [Index("RoleId", Name = "IX_CMS_UserRole_RoleID")]
    [Index("RoleId", "ValidTo", "UserId", Name = "IX_CMS_UserRole_UserID")]
    [Index("UserId", "RoleId", Name = "IX_CMS_UserRole_UserID_RoleID", IsUnique = true)]
    public partial class CmsUserRole
    {
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("RoleID")]
        public int RoleId { get; set; }
        public DateTime? ValidTo { get; set; }
        [Key]
        [Column("UserRoleID")]
        public int UserRoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("CmsUserRoles")]
        public virtual CmsRole Role { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CmsUserRoles")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
