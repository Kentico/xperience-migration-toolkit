using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_AutomationTemplate")]
    [Index("TemplateDisplayName", Name = "IX_CMS_AutomationTemplate_TemplateDisplayName")]
    public partial class CmsAutomationTemplate
    {
        [Key]
        [Column("TemplateID")]
        public int TemplateId { get; set; }
        [StringLength(250)]
        public string TemplateDisplayName { get; set; } = null!;
        public string? TemplateDescription { get; set; }
        [StringLength(200)]
        public string? TemplateIconClass { get; set; }
        public string? TemplateConfiguration { get; set; }
        public Guid TemplateGuid { get; set; }
        public DateTime TemplateLastModified { get; set; }
    }
}
