using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_WorkflowScope")]
    [Index("ScopeClassId", Name = "IX_CMS_WorkflowScope_ScopeClassID")]
    [Index("ScopeCultureId", Name = "IX_CMS_WorkflowScope_ScopeCultureID")]
    [Index("ScopeSiteId", Name = "IX_CMS_WorkflowScope_ScopeSiteID")]
    [Index("ScopeWorkflowId", Name = "IX_CMS_WorkflowScope_ScopeWorkflowID")]
    public partial class CmsWorkflowScope
    {
        [Key]
        [Column("ScopeID")]
        public int ScopeId { get; set; }
        public string ScopeStartingPath { get; set; } = null!;
        [Column("ScopeWorkflowID")]
        public int ScopeWorkflowId { get; set; }
        [Column("ScopeClassID")]
        public int? ScopeClassId { get; set; }
        [Column("ScopeSiteID")]
        public int ScopeSiteId { get; set; }
        [Column("ScopeGUID")]
        public Guid ScopeGuid { get; set; }
        public DateTime ScopeLastModified { get; set; }
        [Column("ScopeCultureID")]
        public int? ScopeCultureId { get; set; }
        public bool? ScopeExcludeChildren { get; set; }
        public bool ScopeExcluded { get; set; }
        public string? ScopeMacroCondition { get; set; }

        [ForeignKey("ScopeClassId")]
        [InverseProperty("CmsWorkflowScopes")]
        public virtual CmsClass? ScopeClass { get; set; }
        [ForeignKey("ScopeCultureId")]
        [InverseProperty("CmsWorkflowScopes")]
        public virtual CmsCulture? ScopeCulture { get; set; }
        [ForeignKey("ScopeSiteId")]
        [InverseProperty("CmsWorkflowScopes")]
        public virtual CmsSite ScopeSite { get; set; } = null!;
        [ForeignKey("ScopeWorkflowId")]
        [InverseProperty("CmsWorkflowScopes")]
        public virtual CmsWorkflow ScopeWorkflow { get; set; } = null!;
    }
}
