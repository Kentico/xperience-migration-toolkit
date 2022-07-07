using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_AutomationHistory")]
    [Index("HistoryApprovedByUserId", Name = "IX_CMS_AutomationHistory_HistoryApprovedByUserID")]
    [Index("HistoryApprovedWhen", Name = "IX_CMS_AutomationHistory_HistoryApprovedWhen")]
    [Index("HistoryStateId", Name = "IX_CMS_AutomationHistory_HistoryStateID")]
    [Index("HistoryStepId", Name = "IX_CMS_AutomationHistory_HistoryStepID")]
    [Index("HistoryTargetStepId", Name = "IX_CMS_AutomationHistory_HistoryTargetStepID")]
    [Index("HistoryWorkflowId", Name = "IX_CMS_AutomationHistory_HistoryWorkflowID")]
    public partial class CmsAutomationHistory
    {
        [Key]
        [Column("HistoryID")]
        public int HistoryId { get; set; }
        [Column("HistoryStepID")]
        public int? HistoryStepId { get; set; }
        [StringLength(440)]
        public string? HistoryStepName { get; set; }
        [StringLength(450)]
        public string HistoryStepDisplayName { get; set; } = null!;
        public int? HistoryStepType { get; set; }
        [Column("HistoryTargetStepID")]
        public int? HistoryTargetStepId { get; set; }
        [StringLength(440)]
        public string? HistoryTargetStepName { get; set; }
        [StringLength(450)]
        public string? HistoryTargetStepDisplayName { get; set; }
        public int? HistoryTargetStepType { get; set; }
        [Column("HistoryApprovedByUserID")]
        public int? HistoryApprovedByUserId { get; set; }
        public DateTime? HistoryApprovedWhen { get; set; }
        public string? HistoryComment { get; set; }
        public int? HistoryTransitionType { get; set; }
        [Column("HistoryWorkflowID")]
        public int HistoryWorkflowId { get; set; }
        public bool? HistoryRejected { get; set; }
        public bool HistoryWasRejected { get; set; }
        [Column("HistoryStateID")]
        public int HistoryStateId { get; set; }

        [ForeignKey("HistoryApprovedByUserId")]
        [InverseProperty("CmsAutomationHistories")]
        public virtual CmsUser? HistoryApprovedByUser { get; set; }
        [ForeignKey("HistoryStateId")]
        [InverseProperty("CmsAutomationHistories")]
        public virtual CmsAutomationState HistoryState { get; set; } = null!;
        [ForeignKey("HistoryStepId")]
        [InverseProperty("CmsAutomationHistoryHistorySteps")]
        public virtual CmsWorkflowStep? HistoryStep { get; set; }
        [ForeignKey("HistoryTargetStepId")]
        [InverseProperty("CmsAutomationHistoryHistoryTargetSteps")]
        public virtual CmsWorkflowStep? HistoryTargetStep { get; set; }
        [ForeignKey("HistoryWorkflowId")]
        [InverseProperty("CmsAutomationHistories")]
        public virtual CmsWorkflow HistoryWorkflow { get; set; } = null!;
    }
}
