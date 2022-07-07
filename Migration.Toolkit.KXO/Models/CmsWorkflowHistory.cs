using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowHistory")]
    [Index("ApprovedByUserId", Name = "IX_CMS_WorkflowHistory_ApprovedByUserID")]
    [Index("ApprovedWhen", Name = "IX_CMS_WorkflowHistory_ApprovedWhen")]
    [Index("HistoryWorkflowId", Name = "IX_CMS_WorkflowHistory_HistoryWorkflowID")]
    [Index("StepId", Name = "IX_CMS_WorkflowHistory_StepID")]
    [Index("TargetStepId", Name = "IX_CMS_WorkflowHistory_TargetStepID")]
    [Index("VersionHistoryId", Name = "IX_CMS_WorkflowHistory_VersionHistoryID")]
    public partial class CmsWorkflowHistory
    {
        [Key]
        [Column("WorkflowHistoryID")]
        public int WorkflowHistoryId { get; set; }
        [Column("VersionHistoryID")]
        public int VersionHistoryId { get; set; }
        [Column("StepID")]
        public int? StepId { get; set; }
        [StringLength(450)]
        public string StepDisplayName { get; set; } = null!;
        [Column("ApprovedByUserID")]
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedWhen { get; set; }
        public string? Comment { get; set; }
        public bool WasRejected { get; set; }
        [StringLength(440)]
        public string? StepName { get; set; }
        [Column("TargetStepID")]
        public int? TargetStepId { get; set; }
        [StringLength(440)]
        public string? TargetStepName { get; set; }
        [StringLength(450)]
        public string? TargetStepDisplayName { get; set; }
        public int? StepType { get; set; }
        public int? TargetStepType { get; set; }
        [StringLength(100)]
        public string? HistoryObjectType { get; set; }
        [Column("HistoryObjectID")]
        public int? HistoryObjectId { get; set; }
        public int? HistoryTransitionType { get; set; }
        [Column("HistoryWorkflowID")]
        public int? HistoryWorkflowId { get; set; }
        public bool? HistoryRejected { get; set; }

        [ForeignKey("ApprovedByUserId")]
        [InverseProperty("CmsWorkflowHistories")]
        public virtual CmsUser? ApprovedByUser { get; set; }
        [ForeignKey("HistoryWorkflowId")]
        [InverseProperty("CmsWorkflowHistories")]
        public virtual CmsWorkflow? HistoryWorkflow { get; set; }
        [ForeignKey("StepId")]
        [InverseProperty("CmsWorkflowHistorySteps")]
        public virtual CmsWorkflowStep? Step { get; set; }
        [ForeignKey("TargetStepId")]
        [InverseProperty("CmsWorkflowHistoryTargetSteps")]
        public virtual CmsWorkflowStep? TargetStep { get; set; }
        [ForeignKey("VersionHistoryId")]
        [InverseProperty("CmsWorkflowHistories")]
        public virtual CmsVersionHistory VersionHistory { get; set; } = null!;
    }
}
