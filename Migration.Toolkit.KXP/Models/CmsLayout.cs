namespace Migration.Toolkit.KXP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("CMS_Layout")]
    [Index("LayoutDisplayName", Name = "IX_CMS_Layout_LayoutDisplayName")]
    public partial class CmsLayout
    {
        public CmsLayout()
        {
            CmsPageTemplates = new HashSet<CmsPageTemplate>();
        }

        [Key]
        [Column("LayoutID")]
        public int LayoutId { get; set; }
        [StringLength(100)]
        public string LayoutCodeName { get; set; } = null!;
        [StringLength(200)]
        public string LayoutDisplayName { get; set; } = null!;
        public string? LayoutDescription { get; set; }
        public string LayoutCode { get; set; } = null!;
        [Column("LayoutVersionGUID")]
        [StringLength(50)]
        public string? LayoutVersionGuid { get; set; }
        [Column("LayoutGUID")]
        public Guid LayoutGuid { get; set; }
        public DateTime LayoutLastModified { get; set; }
        [StringLength(50)]
        public string? LayoutType { get; set; }
        [Column("LayoutCSS")]
        public string? LayoutCss { get; set; }

        [InverseProperty("PageTemplateLayoutNavigation")]
        public virtual ICollection<CmsPageTemplate> CmsPageTemplates { get; set; }
    }
}
