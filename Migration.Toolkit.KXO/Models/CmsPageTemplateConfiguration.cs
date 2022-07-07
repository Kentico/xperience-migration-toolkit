using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_PageTemplateConfiguration")]
    [Index("PageTemplateConfigurationSiteId", Name = "IX_CMS_PageTemplateConfiguration_PageTemplateConfigurationSiteID")]
    public partial class CmsPageTemplateConfiguration
    {
        [Key]
        [Column("PageTemplateConfigurationID")]
        public int PageTemplateConfigurationId { get; set; }
        [Column("PageTemplateConfigurationGUID")]
        public Guid PageTemplateConfigurationGuid { get; set; }
        [Column("PageTemplateConfigurationSiteID")]
        public int PageTemplateConfigurationSiteId { get; set; }
        public DateTime PageTemplateConfigurationLastModified { get; set; }
        [StringLength(200)]
        public string PageTemplateConfigurationName { get; set; } = null!;
        public string? PageTemplateConfigurationDescription { get; set; }
        [Column("PageTemplateConfigurationThumbnailGUID")]
        public Guid? PageTemplateConfigurationThumbnailGuid { get; set; }
        public string PageTemplateConfigurationTemplate { get; set; } = null!;
        public string? PageTemplateConfigurationWidgets { get; set; }

        [ForeignKey("PageTemplateConfigurationSiteId")]
        [InverseProperty("CmsPageTemplateConfigurations")]
        public virtual CmsSite PageTemplateConfigurationSite { get; set; } = null!;
    }
}
