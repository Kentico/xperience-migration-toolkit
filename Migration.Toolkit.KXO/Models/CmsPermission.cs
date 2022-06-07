using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Permission")]
    [Index("ClassId", "PermissionName", Name = "IX_CMS_Permission_ClassID_PermissionName")]
    [Index("ResourceId", "PermissionName", Name = "IX_CMS_Permission_ResourceID_PermissionName")]
    public partial class CmsPermission
    {
        public CmsPermission()
        {
            MediaLibraryRolePermissions = new HashSet<MediaLibraryRolePermission>();
            Roles = new HashSet<CmsRole>();
        }

        [Key]
        [Column("PermissionID")]
        public int PermissionId { get; set; }
        [StringLength(100)]
        public string PermissionDisplayName { get; set; } = null!;
        [StringLength(100)]
        public string PermissionName { get; set; } = null!;
        [Column("ClassID")]
        public int? ClassId { get; set; }
        [Column("ResourceID")]
        public int? ResourceId { get; set; }
        [Column("PermissionGUID")]
        public Guid PermissionGuid { get; set; }
        public DateTime PermissionLastModified { get; set; }
        public string? PermissionDescription { get; set; }
        public bool? PermissionDisplayInMatrix { get; set; }
        public int? PermissionOrder { get; set; }
        public bool? PermissionEditableByGlobalAdmin { get; set; }

        [ForeignKey("ClassId")]
        [InverseProperty("CmsPermissions")]
        public virtual CmsClass? Class { get; set; }
        [ForeignKey("ResourceId")]
        [InverseProperty("CmsPermissions")]
        public virtual CmsResource? Resource { get; set; }
        [InverseProperty("Permission")]
        public virtual ICollection<MediaLibraryRolePermission> MediaLibraryRolePermissions { get; set; }

        [ForeignKey("PermissionId")]
        [InverseProperty("Permissions")]
        public virtual ICollection<CmsRole> Roles { get; set; }
    }
}
