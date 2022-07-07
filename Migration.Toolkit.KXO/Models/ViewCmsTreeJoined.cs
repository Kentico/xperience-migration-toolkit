using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Keyless]
    public partial class ViewCmsTreeJoined
    {
        [StringLength(100)]
        public string ClassName { get; set; } = null!;
        [StringLength(100)]
        public string ClassDisplayName { get; set; } = null!;
        [Column("NodeID")]
        public int NodeId { get; set; }
        [StringLength(450)]
        public string NodeAliasPath { get; set; } = null!;
        [StringLength(100)]
        public string NodeName { get; set; } = null!;
        [StringLength(50)]
        public string NodeAlias { get; set; } = null!;
        [Column("NodeClassID")]
        public int NodeClassId { get; set; }
        [Column("NodeParentID")]
        public int? NodeParentId { get; set; }
        public int NodeLevel { get; set; }
        [Column("NodeSiteID")]
        public int NodeSiteId { get; set; }
        [Column("NodeGUID")]
        public Guid NodeGuid { get; set; }
        public int? NodeOrder { get; set; }
        public bool? IsSecuredNode { get; set; }
        [Column("NodeLinkedNodeID")]
        public int? NodeLinkedNodeId { get; set; }
        public int? NodeOwner { get; set; }
        public string? NodeCustomData { get; set; }
        [Column("NodeLinkedNodeSiteID")]
        public int? NodeLinkedNodeSiteId { get; set; }
        public bool? NodeHasChildren { get; set; }
        public bool? NodeHasLinks { get; set; }
        [Column("NodeOriginalNodeID")]
        public int? NodeOriginalNodeId { get; set; }
        [Column("DocumentID")]
        public int DocumentId { get; set; }
        [StringLength(100)]
        public string DocumentName { get; set; } = null!;
        public DateTime? DocumentModifiedWhen { get; set; }
        [Column("DocumentModifiedByUserID")]
        public int? DocumentModifiedByUserId { get; set; }
        public int? DocumentForeignKeyValue { get; set; }
        [Column("DocumentCreatedByUserID")]
        public int? DocumentCreatedByUserId { get; set; }
        public DateTime? DocumentCreatedWhen { get; set; }
        [Column("DocumentCheckedOutByUserID")]
        public int? DocumentCheckedOutByUserId { get; set; }
        public DateTime? DocumentCheckedOutWhen { get; set; }
        [Column("DocumentCheckedOutVersionHistoryID")]
        public int? DocumentCheckedOutVersionHistoryId { get; set; }
        [Column("DocumentPublishedVersionHistoryID")]
        public int? DocumentPublishedVersionHistoryId { get; set; }
        [Column("DocumentWorkflowStepID")]
        public int? DocumentWorkflowStepId { get; set; }
        public DateTime? DocumentPublishFrom { get; set; }
        public DateTime? DocumentPublishTo { get; set; }
        [StringLength(50)]
        public string DocumentCulture { get; set; } = null!;
        [Column("DocumentNodeID")]
        public int DocumentNodeId { get; set; }
        public string? DocumentContent { get; set; }
        public string? DocumentCustomData { get; set; }
        public DateTime? DocumentLastPublished { get; set; }
        public bool? DocumentSearchExcluded { get; set; }
        [StringLength(50)]
        public string? DocumentLastVersionNumber { get; set; }
        public bool? DocumentIsArchived { get; set; }
        [Column("DocumentGUID")]
        public Guid? DocumentGuid { get; set; }
        [Column("DocumentWorkflowCycleGUID")]
        public Guid? DocumentWorkflowCycleGuid { get; set; }
        [StringLength(450)]
        public string? DocumentWorkflowActionStatus { get; set; }
        public bool DocumentCanBePublished { get; set; }
        public string? DocumentPageBuilderWidgets { get; set; }
        public string? DocumentPageTemplateConfiguration { get; set; }
        [Column("DocumentABTestConfiguration")]
        public string? DocumentAbtestConfiguration { get; set; }
        public bool DocumentShowInMenu { get; set; }
    }
}
