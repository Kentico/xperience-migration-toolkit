using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_EmailTemplate")]
    [Index("EmailTemplateName", "EmailTemplateSiteId", Name = "IX_CMS_EmailTemplate_EmailTemplateName_EmailTemplateSiteID")]
    [Index("EmailTemplateSiteId", Name = "IX_CMS_EmailTemplate_EmailTemplateSiteID")]
    public partial class CmsEmailTemplate
    {
        [Key]
        [Column("EmailTemplateID")]
        public int EmailTemplateId { get; set; }
        [StringLength(200)]
        public string EmailTemplateName { get; set; } = null!;
        [StringLength(200)]
        public string EmailTemplateDisplayName { get; set; } = null!;
        public string? EmailTemplateText { get; set; }
        [Column("EmailTemplateSiteID")]
        public int? EmailTemplateSiteId { get; set; }
        [Column("EmailTemplateGUID")]
        public Guid EmailTemplateGuid { get; set; }
        public DateTime EmailTemplateLastModified { get; set; }
        public string? EmailTemplatePlainText { get; set; }
        [StringLength(250)]
        public string? EmailTemplateSubject { get; set; }
        [StringLength(254)]
        public string? EmailTemplateFrom { get; set; }
        [StringLength(998)]
        public string? EmailTemplateCc { get; set; }
        [StringLength(998)]
        public string? EmailTemplateBcc { get; set; }
        [StringLength(100)]
        public string? EmailTemplateType { get; set; }
        public string? EmailTemplateDescription { get; set; }
        [StringLength(254)]
        public string? EmailTemplateReplyTo { get; set; }

        [ForeignKey("EmailTemplateSiteId")]
        [InverseProperty("CmsEmailTemplates")]
        public virtual CmsSite? EmailTemplateSite { get; set; }
    }
}
