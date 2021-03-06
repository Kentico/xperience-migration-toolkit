namespace Migration.Toolkit.KXP.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("Analytics_CampaignAsset")]
    [Index("CampaignAssetCampaignId", Name = "IX_Analytics_CampaignAsset_CampaignAssetCampaignID")]
    public partial class AnalyticsCampaignAsset
    {
        public AnalyticsCampaignAsset()
        {
            AnalyticsCampaignAssetUrls = new HashSet<AnalyticsCampaignAssetUrl>();
        }

        [Key]
        [Column("CampaignAssetID")]
        public int CampaignAssetId { get; set; }
        public Guid CampaignAssetGuid { get; set; }
        public DateTime CampaignAssetLastModified { get; set; }
        public Guid CampaignAssetAssetGuid { get; set; }
        [Column("CampaignAssetCampaignID")]
        public int CampaignAssetCampaignId { get; set; }
        [StringLength(200)]
        public string CampaignAssetType { get; set; } = null!;

        [ForeignKey("CampaignAssetCampaignId")]
        [InverseProperty("AnalyticsCampaignAssets")]
        public virtual AnalyticsCampaign CampaignAssetCampaign { get; set; } = null!;
        [InverseProperty("CampaignAssetUrlCampaignAsset")]
        public virtual ICollection<AnalyticsCampaignAssetUrl> AnalyticsCampaignAssetUrls { get; set; }
    }
}
