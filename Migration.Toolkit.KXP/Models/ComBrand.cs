namespace Migration.Toolkit.KXP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("COM_Brand")]
    [Index("BrandDisplayName", Name = "IX_COM_Brand_BrandDisplayName")]
    [Index("BrandSiteId", "BrandEnabled", Name = "IX_COM_Brand_BrandSiteID_BrandEnabled")]
    public partial class ComBrand
    {
        public ComBrand()
        {
            ComMultiBuyDiscountBrands = new HashSet<ComMultiBuyDiscountBrand>();
            ComSkus = new HashSet<ComSku>();
        }

        [Key]
        [Column("BrandID")]
        public int BrandId { get; set; }
        [StringLength(200)]
        public string BrandDisplayName { get; set; } = null!;
        [StringLength(200)]
        public string BrandName { get; set; } = null!;
        public string? BrandDescription { get; set; }
        [StringLength(400)]
        public string? BrandHomepage { get; set; }
        [Column("BrandThumbnailGUID")]
        public Guid? BrandThumbnailGuid { get; set; }
        [Column("BrandSiteID")]
        public int BrandSiteId { get; set; }
        [Required]
        public bool? BrandEnabled { get; set; }
        public Guid BrandGuid { get; set; }
        public DateTime BrandLastModified { get; set; }

        [ForeignKey("BrandSiteId")]
        [InverseProperty("ComBrands")]
        public virtual CmsSite BrandSite { get; set; } = null!;
        [InverseProperty("Brand")]
        public virtual ICollection<ComMultiBuyDiscountBrand> ComMultiBuyDiscountBrands { get; set; }
        [InverseProperty("Skubrand")]
        public virtual ICollection<ComSku> ComSkus { get; set; }
    }
}
