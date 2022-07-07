using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowTransition")]
    [Index("TransitionEndStepId", Name = "IX_CMS_WorkflowTransition_TransitionEndStepID")]
    [Index("TransitionStartStepId", "TransitionSourcePointGuid", "TransitionEndStepId", Name = "IX_CMS_WorkflowTransition_TransitionStartStepID_TransitionSourcePointGUID_TransitionEndStepID", IsUnique = true)]
    [Index("TransitionWorkflowId", Name = "IX_CMS_WorkflowTransition_TransitionWorkflowID")]
    public partial class CmsWorkflowTransition
    {
        [Key]
        [Column("TransitionID")]
        public int TransitionId { get; set; }
        [Column("TransitionStartStepID")]
        public int TransitionStartStepId { get; set; }
        [Column("TransitionEndStepID")]
        public int TransitionEndStepId { get; set; }
        public int TransitionType { get; set; }
        public DateTime TransitionLastModified { get; set; }
        [Column("TransitionSourcePointGUID")]
        public Guid? TransitionSourcePointGuid { get; set; }
        [Column("TransitionWorkflowID")]
        public int TransitionWorkflowId { get; set; }

        [ForeignKey("TransitionEndStepId")]
        [InverseProperty("CmsWorkflowTransitionTransitionEndSteps")]
        public virtual CmsWorkflowStep TransitionEndStep { get; set; } = null!;
        [ForeignKey("TransitionStartStepId")]
        [InverseProperty("CmsWorkflowTransitionTransitionStartSteps")]
        public virtual CmsWorkflowStep TransitionStartStep { get; set; } = null!;
        [ForeignKey("TransitionWorkflowId")]
        [InverseProperty("CmsWorkflowTransitions")]
        public virtual CmsWorkflow TransitionWorkflow { get; set; } = null!;
    }
}
