using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Personalization")]
    [Index("PersonalizationSiteId", Name = "IX_CMS_Personalization_PersonalizationSiteID_SiteID")]
    [Index("PersonalizationUserId", Name = "IX_CMS_Personalization_PersonalizationUserID")]
    public partial class CmsPersonalization
    {
        [Key]
        [Column("PersonalizationID")]
        public int PersonalizationId { get; set; }
        [Column("PersonalizationGUID")]
        public Guid PersonalizationGuid { get; set; }
        public DateTime PersonalizationLastModified { get; set; }
        [Column("PersonalizationUserID")]
        public int? PersonalizationUserId { get; set; }
        public string? PersonalizationWebParts { get; set; }
        [StringLength(200)]
        public string? PersonalizationDashboardName { get; set; }
        [Column("PersonalizationSiteID")]
        public int? PersonalizationSiteId { get; set; }

        [ForeignKey("PersonalizationSiteId")]
        [InverseProperty("CmsPersonalizations")]
        public virtual CmsSite? PersonalizationSite { get; set; }
        [ForeignKey("PersonalizationUserId")]
        [InverseProperty("CmsPersonalizations")]
        public virtual CmsUser? PersonalizationUser { get; set; }
    }
}
