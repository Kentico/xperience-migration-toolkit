namespace Migration.Toolkit.KXP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("COM_OptionCategory")]
    [Index("CategorySiteId", Name = "IX_COM_OptionCategory_CategorySiteID")]
    public partial class ComOptionCategory
    {
        public ComOptionCategory()
        {
            ComSkuoptionCategories = new HashSet<ComSkuoptionCategory>();
            ComSkus = new HashSet<ComSku>();
        }

        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [StringLength(200)]
        public string CategoryDisplayName { get; set; } = null!;
        [StringLength(200)]
        public string CategoryName { get; set; } = null!;
        [StringLength(200)]
        public string CategorySelectionType { get; set; } = null!;
        [StringLength(200)]
        public string? CategoryDefaultOptions { get; set; }
        public string? CategoryDescription { get; set; }
        [StringLength(200)]
        public string? CategoryDefaultRecord { get; set; }
        [Required]
        public bool? CategoryEnabled { get; set; }
        [Column("CategoryGUID")]
        public Guid CategoryGuid { get; set; }
        public DateTime CategoryLastModified { get; set; }
        public bool? CategoryDisplayPrice { get; set; }
        [Column("CategorySiteID")]
        public int? CategorySiteId { get; set; }
        public int? CategoryTextMaxLength { get; set; }
        [StringLength(20)]
        public string? CategoryType { get; set; }
        public int? CategoryTextMinLength { get; set; }
        [StringLength(200)]
        public string? CategoryLiveSiteDisplayName { get; set; }

        [ForeignKey("CategorySiteId")]
        [InverseProperty("ComOptionCategories")]
        public virtual CmsSite? CategorySite { get; set; }
        [InverseProperty("Category")]
        public virtual ICollection<ComSkuoptionCategory> ComSkuoptionCategories { get; set; }
        [InverseProperty("SkuoptionCategory")]
        public virtual ICollection<ComSku> ComSkus { get; set; }
    }
}
