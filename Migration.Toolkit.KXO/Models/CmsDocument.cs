using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Document")]
    [Index("DocumentCheckedOutByUserId", Name = "IX_CMS_Document_DocumentCheckedOutByUserID")]
    [Index("DocumentCheckedOutVersionHistoryId", Name = "IX_CMS_Document_DocumentCheckedOutVersionHistoryID")]
    [Index("DocumentCreatedByUserId", Name = "IX_CMS_Document_DocumentCreatedByUserID")]
    [Index("DocumentCulture", Name = "IX_CMS_Document_DocumentCulture")]
    [Index("DocumentForeignKeyValue", "DocumentId", "DocumentNodeId", Name = "IX_CMS_Document_DocumentForeignKeyValue_DocumentID_DocumentNodeID")]
    [Index("DocumentModifiedByUserId", Name = "IX_CMS_Document_DocumentModifiedByUserID")]
    [Index("DocumentNodeId", "DocumentId", "DocumentCulture", Name = "IX_CMS_Document_DocumentNodeID_DocumentID_DocumentCulture", IsUnique = true)]
    [Index("DocumentPublishedVersionHistoryId", Name = "IX_CMS_Document_DocumentPublishedVersionHistoryID")]
    [Index("DocumentWorkflowStepId", Name = "IX_CMS_Document_DocumentWorkflowStepID")]
    public partial class CmsDocument
    {
        public CmsDocument()
        {
            CmsAlternativeUrls = new HashSet<CmsAlternativeUrl>();
        }

        [Key]
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
        [Required]
        public bool? DocumentCanBePublished { get; set; }
        public string? DocumentPageBuilderWidgets { get; set; }
        public string? DocumentPageTemplateConfiguration { get; set; }
        [Column("DocumentABTestConfiguration")]
        public string? DocumentAbtestConfiguration { get; set; }
        public bool DocumentShowInMenu { get; set; }

        [ForeignKey("DocumentCheckedOutByUserId")]
        [InverseProperty("CmsDocumentDocumentCheckedOutByUsers")]
        public virtual CmsUser? DocumentCheckedOutByUser { get; set; }
        [ForeignKey("DocumentCheckedOutVersionHistoryId")]
        [InverseProperty("CmsDocumentDocumentCheckedOutVersionHistories")]
        public virtual CmsVersionHistory? DocumentCheckedOutVersionHistory { get; set; }
        [ForeignKey("DocumentCreatedByUserId")]
        [InverseProperty("CmsDocumentDocumentCreatedByUsers")]
        public virtual CmsUser? DocumentCreatedByUser { get; set; }
        [ForeignKey("DocumentModifiedByUserId")]
        [InverseProperty("CmsDocumentDocumentModifiedByUsers")]
        public virtual CmsUser? DocumentModifiedByUser { get; set; }
        [ForeignKey("DocumentNodeId")]
        [InverseProperty("CmsDocuments")]
        public virtual CmsTree DocumentNode { get; set; } = null!;
        [ForeignKey("DocumentPublishedVersionHistoryId")]
        [InverseProperty("CmsDocumentDocumentPublishedVersionHistories")]
        public virtual CmsVersionHistory? DocumentPublishedVersionHistory { get; set; }
        [ForeignKey("DocumentWorkflowStepId")]
        [InverseProperty("CmsDocuments")]
        public virtual CmsWorkflowStep? DocumentWorkflowStep { get; set; }
        [InverseProperty("AlternativeUrlDocument")]
        public virtual ICollection<CmsAlternativeUrl> CmsAlternativeUrls { get; set; }
    }
}
