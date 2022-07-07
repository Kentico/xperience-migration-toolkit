using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_ObjectWorkflowTrigger")]
    [Index("TriggerWorkflowId", Name = "IX_CMS_ObjectWorkflowTrigger_TriggerWorkflowID")]
    public partial class CmsObjectWorkflowTrigger
    {
        [Key]
        [Column("TriggerID")]
        public int TriggerId { get; set; }
        [Column("TriggerGUID")]
        public Guid TriggerGuid { get; set; }
        public DateTime TriggerLastModified { get; set; }
        public int TriggerType { get; set; }
        public string? TriggerMacroCondition { get; set; }
        [Column("TriggerWorkflowID")]
        public int TriggerWorkflowId { get; set; }
        [StringLength(450)]
        public string TriggerDisplayName { get; set; } = null!;
        [StringLength(100)]
        public string TriggerObjectType { get; set; } = null!;
        public string? TriggerParameters { get; set; }
        [StringLength(100)]
        public string? TriggerTargetObjectType { get; set; }
        [Column("TriggerTargetObjectID")]
        public int? TriggerTargetObjectId { get; set; }

        [ForeignKey("TriggerWorkflowId")]
        [InverseProperty("CmsObjectWorkflowTriggers")]
        public virtual CmsWorkflow TriggerWorkflow { get; set; } = null!;
    }
}
