using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("Media_LibraryRolePermission")]
    [Index("PermissionId", Name = "IX_Media_LibraryRolePermission_PermissionID")]
    [Index("RoleId", Name = "IX_Media_LibraryRolePermission_RoleID")]
    public partial class MediaLibraryRolePermission
    {
        [Key]
        [Column("LibraryID")]
        public int LibraryId { get; set; }
        [Key]
        [Column("RoleID")]
        public int RoleId { get; set; }
        [Key]
        [Column("PermissionID")]
        public int PermissionId { get; set; }

        [ForeignKey("LibraryId")]
        [InverseProperty("MediaLibraryRolePermissions")]
        public virtual MediaLibrary Library { get; set; } = null!;
        [ForeignKey("PermissionId")]
        [InverseProperty("MediaLibraryRolePermissions")]
        public virtual CmsPermission Permission { get; set; } = null!;
        [ForeignKey("RoleId")]
        [InverseProperty("MediaLibraryRolePermissions")]
        public virtual CmsRole Role { get; set; } = null!;
    }
}
