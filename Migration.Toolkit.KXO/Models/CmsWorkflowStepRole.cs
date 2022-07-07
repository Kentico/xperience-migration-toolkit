using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowStepRoles")]
    [Index("RoleId", Name = "IX_CMS_WorkflowStepRoles_RoleID")]
    public partial class CmsWorkflowStepRole
    {
        [Key]
        [Column("WorkflowStepRoleID")]
        public int WorkflowStepRoleId { get; set; }
        [Column("StepID")]
        public int StepId { get; set; }
        [Column("RoleID")]
        public int RoleId { get; set; }
        [Column("StepSourcePointGUID")]
        public Guid? StepSourcePointGuid { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("CmsWorkflowStepRoles")]
        public virtual CmsRole Role { get; set; } = null!;
        [ForeignKey("StepId")]
        [InverseProperty("CmsWorkflowStepRoles")]
        public virtual CmsWorkflowStep Step { get; set; } = null!;
    }
}
