using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Workflow")]
    public partial class CmsWorkflow
    {
        public CmsWorkflow()
        {
            CmsAutomationHistories = new HashSet<CmsAutomationHistory>();
            CmsAutomationStates = new HashSet<CmsAutomationState>();
            CmsObjectWorkflowTriggers = new HashSet<CmsObjectWorkflowTrigger>();
            CmsVersionHistories = new HashSet<CmsVersionHistory>();
            CmsWorkflowHistories = new HashSet<CmsWorkflowHistory>();
            CmsWorkflowScopes = new HashSet<CmsWorkflowScope>();
            CmsWorkflowSteps = new HashSet<CmsWorkflowStep>();
            CmsWorkflowTransitions = new HashSet<CmsWorkflowTransition>();
            Users = new HashSet<CmsUser>();
        }

        [Key]
        [Column("WorkflowID")]
        public int WorkflowId { get; set; }
        public string WorkflowDisplayName { get; set; } = null!;
        [StringLength(450)]
        public string WorkflowName { get; set; } = null!;
        [Column("WorkflowGUID")]
        public Guid WorkflowGuid { get; set; }
        public DateTime WorkflowLastModified { get; set; }
        public bool? WorkflowAutoPublishChanges { get; set; }
        public bool? WorkflowUseCheckinCheckout { get; set; }
        public int? WorkflowType { get; set; }
        public bool? WorkflowSendEmails { get; set; }
        public bool? WorkflowSendApproveEmails { get; set; }
        public bool? WorkflowSendRejectEmails { get; set; }
        public bool? WorkflowSendPublishEmails { get; set; }
        public bool? WorkflowSendArchiveEmails { get; set; }
        [StringLength(200)]
        public string? WorkflowApprovedTemplateName { get; set; }
        [StringLength(200)]
        public string? WorkflowRejectedTemplateName { get; set; }
        [StringLength(200)]
        public string? WorkflowPublishedTemplateName { get; set; }
        [StringLength(200)]
        public string? WorkflowArchivedTemplateName { get; set; }
        public bool? WorkflowSendReadyForApprovalEmails { get; set; }
        [StringLength(200)]
        public string? WorkflowReadyForApprovalTemplateName { get; set; }
        [StringLength(200)]
        public string? WorkflowNotificationTemplateName { get; set; }
        public string? WorkflowAllowedObjects { get; set; }
        public int? WorkflowRecurrenceType { get; set; }
        [Required]
        public bool? WorkflowEnabled { get; set; }

        [InverseProperty("HistoryWorkflow")]
        public virtual ICollection<CmsAutomationHistory> CmsAutomationHistories { get; set; }
        [InverseProperty("StateWorkflow")]
        public virtual ICollection<CmsAutomationState> CmsAutomationStates { get; set; }
        [InverseProperty("TriggerWorkflow")]
        public virtual ICollection<CmsObjectWorkflowTrigger> CmsObjectWorkflowTriggers { get; set; }
        [InverseProperty("VersionWorkflow")]
        public virtual ICollection<CmsVersionHistory> CmsVersionHistories { get; set; }
        [InverseProperty("HistoryWorkflow")]
        public virtual ICollection<CmsWorkflowHistory> CmsWorkflowHistories { get; set; }
        [InverseProperty("ScopeWorkflow")]
        public virtual ICollection<CmsWorkflowScope> CmsWorkflowScopes { get; set; }
        [InverseProperty("StepWorkflow")]
        public virtual ICollection<CmsWorkflowStep> CmsWorkflowSteps { get; set; }
        [InverseProperty("TransitionWorkflow")]
        public virtual ICollection<CmsWorkflowTransition> CmsWorkflowTransitions { get; set; }

        [ForeignKey("WorkflowId")]
        [InverseProperty("Workflows")]
        public virtual ICollection<CmsUser> Users { get; set; }
    }
}
