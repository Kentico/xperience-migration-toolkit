using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_AutomationState")]
    [Index("StateObjectId", "StateObjectType", Name = "IX_CMS_AutomationState_StateObjectID_StateObjectType")]
    [Index("StateSiteId", Name = "IX_CMS_AutomationState_StateSiteID")]
    [Index("StateStepId", Name = "IX_CMS_AutomationState_StateStepID")]
    [Index("StateUserId", Name = "IX_CMS_AutomationState_StateUserID")]
    [Index("StateWorkflowId", Name = "IX_CMS_AutomationState_StateWorkflowID")]
    public partial class CmsAutomationState
    {
        public CmsAutomationState()
        {
            CmsAutomationHistories = new HashSet<CmsAutomationHistory>();
        }

        [Key]
        [Column("StateID")]
        public int StateId { get; set; }
        [Column("StateStepID")]
        public int StateStepId { get; set; }
        [Column("StateObjectID")]
        public int StateObjectId { get; set; }
        [StringLength(100)]
        public string StateObjectType { get; set; } = null!;
        [StringLength(450)]
        public string? StateActionStatus { get; set; }
        public DateTime? StateCreated { get; set; }
        public DateTime? StateLastModified { get; set; }
        [Column("StateWorkflowID")]
        public int StateWorkflowId { get; set; }
        public int? StateStatus { get; set; }
        [Column("StateSiteID")]
        public int? StateSiteId { get; set; }
        [Column("StateUserID")]
        public int? StateUserId { get; set; }
        [Column("StateGUID")]
        public Guid StateGuid { get; set; }
        public string? StateCustomData { get; set; }

        [ForeignKey("StateSiteId")]
        [InverseProperty("CmsAutomationStates")]
        public virtual CmsSite? StateSite { get; set; }
        [ForeignKey("StateStepId")]
        [InverseProperty("CmsAutomationStates")]
        public virtual CmsWorkflowStep StateStep { get; set; } = null!;
        [ForeignKey("StateUserId")]
        [InverseProperty("CmsAutomationStates")]
        public virtual CmsUser? StateUser { get; set; }
        [ForeignKey("StateWorkflowId")]
        [InverseProperty("CmsAutomationStates")]
        public virtual CmsWorkflow StateWorkflow { get; set; } = null!;
        [InverseProperty("HistoryState")]
        public virtual ICollection<CmsAutomationHistory> CmsAutomationHistories { get; set; }
    }
}
