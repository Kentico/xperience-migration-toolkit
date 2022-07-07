using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowAction")]
    [Index("ActionResourceId", Name = "IX_CMS_WorkflowAction_ActionResourceID")]
    public partial class CmsWorkflowAction
    {
        public CmsWorkflowAction()
        {
            CmsWorkflowSteps = new HashSet<CmsWorkflowStep>();
        }

        [Key]
        [Column("ActionID")]
        public int ActionId { get; set; }
        [StringLength(200)]
        public string ActionDisplayName { get; set; } = null!;
        [StringLength(200)]
        public string ActionName { get; set; } = null!;
        public string? ActionParameters { get; set; }
        public string? ActionDescription { get; set; }
        [StringLength(200)]
        public string ActionAssemblyName { get; set; } = null!;
        [StringLength(200)]
        public string ActionClass { get; set; } = null!;
        [Column("ActionResourceID")]
        public int? ActionResourceId { get; set; }
        [Column("ActionThumbnailGUID")]
        public Guid? ActionThumbnailGuid { get; set; }
        [Column("ActionGUID")]
        public Guid ActionGuid { get; set; }
        public DateTime ActionLastModified { get; set; }
        [Required]
        public bool? ActionEnabled { get; set; }
        public string? ActionAllowedObjects { get; set; }
        [Column("ActionIconGUID")]
        public Guid? ActionIconGuid { get; set; }
        public int? ActionWorkflowType { get; set; }
        [StringLength(200)]
        public string? ActionIconClass { get; set; }
        [StringLength(200)]
        public string? ActionThumbnailClass { get; set; }
        [StringLength(200)]
        public string? ActionDataProviderClass { get; set; }
        [StringLength(200)]
        public string? ActionDataProviderAssemblyName { get; set; }

        [ForeignKey("ActionResourceId")]
        [InverseProperty("CmsWorkflowActions")]
        public virtual CmsResource? ActionResource { get; set; }
        [InverseProperty("StepAction")]
        public virtual ICollection<CmsWorkflowStep> CmsWorkflowSteps { get; set; }
    }
}
