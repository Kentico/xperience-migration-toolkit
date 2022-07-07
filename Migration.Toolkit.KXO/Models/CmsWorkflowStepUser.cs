using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowStepUser")]
    [Index("UserId", Name = "IX_CMS_WorkflowStepUser_UserID")]
    public partial class CmsWorkflowStepUser
    {
        [Key]
        [Column("WorkflowStepUserID")]
        public int WorkflowStepUserId { get; set; }
        [Column("StepID")]
        public int StepId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [Column("StepSourcePointGUID")]
        public Guid? StepSourcePointGuid { get; set; }

        [ForeignKey("StepId")]
        [InverseProperty("CmsWorkflowStepUsers")]
        public virtual CmsWorkflowStep Step { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CmsWorkflowStepUsers")]
        public virtual CmsUser User { get; set; } = null!;
    }
}
