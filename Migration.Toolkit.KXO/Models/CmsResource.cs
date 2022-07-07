using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Resource")]
    [Index("ResourceName", Name = "IX_CMS_Resource_ResourceName")]
    public partial class CmsResource
    {
        public CmsResource()
        {
            CmsClasses = new HashSet<CmsClass>();
            CmsPermissions = new HashSet<CmsPermission>();
            CmsResourceLibraries = new HashSet<CmsResourceLibrary>();
            CmsScheduledTasks = new HashSet<CmsScheduledTask>();
            CmsSettingsCategories = new HashSet<CmsSettingsCategory>();
            CmsWorkflowActions = new HashSet<CmsWorkflowAction>();
            Sites = new HashSet<CmsSite>();
        }

        [Key]
        [Column("ResourceID")]
        public int ResourceId { get; set; }
        [StringLength(100)]
        public string ResourceDisplayName { get; set; } = null!;
        [StringLength(100)]
        public string ResourceName { get; set; } = null!;
        public string? ResourceDescription { get; set; }
        public bool? ShowInDevelopment { get; set; }
        [Column("ResourceURL")]
        [StringLength(1000)]
        public string? ResourceUrl { get; set; }
        [Column("ResourceGUID")]
        public Guid ResourceGuid { get; set; }
        public DateTime ResourceLastModified { get; set; }
        public bool? ResourceIsInDevelopment { get; set; }
        public bool? ResourceHasFiles { get; set; }
        [StringLength(200)]
        public string? ResourceVersion { get; set; }
        [StringLength(200)]
        public string? ResourceAuthor { get; set; }
        [StringLength(50)]
        public string? ResourceInstallationState { get; set; }
        [StringLength(50)]
        public string? ResourceInstalledVersion { get; set; }

        [InverseProperty("ClassResource")]
        public virtual ICollection<CmsClass> CmsClasses { get; set; }
        [InverseProperty("Resource")]
        public virtual ICollection<CmsPermission> CmsPermissions { get; set; }
        [InverseProperty("ResourceLibraryResource")]
        public virtual ICollection<CmsResourceLibrary> CmsResourceLibraries { get; set; }
        [InverseProperty("TaskResource")]
        public virtual ICollection<CmsScheduledTask> CmsScheduledTasks { get; set; }
        [InverseProperty("CategoryResource")]
        public virtual ICollection<CmsSettingsCategory> CmsSettingsCategories { get; set; }
        [InverseProperty("ActionResource")]
        public virtual ICollection<CmsWorkflowAction> CmsWorkflowActions { get; set; }

        [ForeignKey("ResourceId")]
        [InverseProperty("Resources")]
        public virtual ICollection<CmsSite> Sites { get; set; }
    }
}
