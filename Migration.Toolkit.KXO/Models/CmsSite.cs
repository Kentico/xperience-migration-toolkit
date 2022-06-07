using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Migration.Toolkit.KXO.Models
{
    [Table("CMS_Site")]
    [Index("SiteDomainName", "SiteStatus", Name = "IX_CMS_Site_SiteDomainName_SiteStatus")]
    [Index("SiteName", Name = "IX_CMS_Site_SiteName")]
    public partial class CmsSite
    {
        public CmsSite()
        {
            AnalyticsCampaigns = new HashSet<AnalyticsCampaign>();
            AnalyticsExitPages = new HashSet<AnalyticsExitPage>();
            AnalyticsStatistics = new HashSet<AnalyticsStatistic>();
            CmsAcls = new HashSet<CmsAcl>();
            CmsAlternativeUrls = new HashSet<CmsAlternativeUrl>();
            CmsAttachmentHistories = new HashSet<CmsAttachmentHistory>();
            CmsAttachments = new HashSet<CmsAttachment>();
            CmsAutomationStates = new HashSet<CmsAutomationState>();
            CmsCategories = new HashSet<CmsCategory>();
            CmsDocumentTypeScopes = new HashSet<CmsDocumentTypeScope>();
            CmsEmailTemplates = new HashSet<CmsEmailTemplate>();
            CmsForms = new HashSet<CmsForm>();
            CmsMemberships = new HashSet<CmsMembership>();
            CmsMetaFiles = new HashSet<CmsMetaFile>();
            CmsObjectVersionHistories = new HashSet<CmsObjectVersionHistory>();
            CmsPageFormerUrlPaths = new HashSet<CmsPageFormerUrlPath>();
            CmsPageTemplateConfigurations = new HashSet<CmsPageTemplateConfiguration>();
            CmsPageUrlPaths = new HashSet<CmsPageUrlPath>();
            CmsPersonalizations = new HashSet<CmsPersonalization>();
            CmsRoles = new HashSet<CmsRole>();
            CmsScheduledTasks = new HashSet<CmsScheduledTask>();
            CmsSettingsKeys = new HashSet<CmsSettingsKey>();
            CmsSiteDomainAliases = new HashSet<CmsSiteDomainAlias>();
            CmsTagGroups = new HashSet<CmsTagGroup>();
            CmsTreeNodeLinkedNodeSites = new HashSet<CmsTree>();
            CmsTreeNodeSites = new HashSet<CmsTree>();
            CmsUserCultures = new HashSet<CmsUserCulture>();
            CmsUserSites = new HashSet<CmsUserSite>();
            CmsVersionHistories = new HashSet<CmsVersionHistory>();
            CmsWorkflowScopes = new HashSet<CmsWorkflowScope>();
            ComBrands = new HashSet<ComBrand>();
            ComCarriers = new HashSet<ComCarrier>();
            ComCollections = new HashSet<ComCollection>();
            ComCurrencies = new HashSet<ComCurrency>();
            ComCustomerCreditHistories = new HashSet<ComCustomerCreditHistory>();
            ComCustomers = new HashSet<ComCustomer>();
            ComDepartments = new HashSet<ComDepartment>();
            ComDiscounts = new HashSet<ComDiscount>();
            ComExchangeTables = new HashSet<ComExchangeTable>();
            ComGiftCards = new HashSet<ComGiftCard>();
            ComInternalStatuses = new HashSet<ComInternalStatus>();
            ComManufacturers = new HashSet<ComManufacturer>();
            ComMultiBuyDiscounts = new HashSet<ComMultiBuyDiscount>();
            ComOptionCategories = new HashSet<ComOptionCategory>();
            ComOrderStatuses = new HashSet<ComOrderStatus>();
            ComOrders = new HashSet<ComOrder>();
            ComPaymentOptions = new HashSet<ComPaymentOption>();
            ComPublicStatuses = new HashSet<ComPublicStatus>();
            ComShippingOptions = new HashSet<ComShippingOption>();
            ComShoppingCarts = new HashSet<ComShoppingCart>();
            ComSkus = new HashSet<ComSku>();
            ComSuppliers = new HashSet<ComSupplier>();
            ComTaxClasses = new HashSet<ComTaxClass>();
            ComWishlists = new HashSet<ComWishlist>();
            ExportHistories = new HashSet<ExportHistory>();
            ExportTasks = new HashSet<ExportTask>();
            IntegrationTasks = new HashSet<IntegrationTask>();
            MediaFiles = new HashSet<MediaFile>();
            MediaLibraries = new HashSet<MediaLibrary>();
            NewsletterEmailTemplates = new HashSet<NewsletterEmailTemplate>();
            NewsletterEmailWidgets = new HashSet<NewsletterEmailWidget>();
            NewsletterEmails = new HashSet<NewsletterEmail>();
            NewsletterNewsletterIssues = new HashSet<NewsletterNewsletterIssue>();
            NewsletterNewsletters = new HashSet<NewsletterNewsletter>();
            NewsletterSubscribers = new HashSet<NewsletterSubscriber>();
            OmAbtests = new HashSet<OmAbtest>();
            OmTrackedWebsites = new HashSet<OmTrackedWebsite>();
            StagingServers = new HashSet<StagingServer>();
            StagingTasks = new HashSet<StagingTask>();
            Classes = new HashSet<CmsClass>();
            Cultures = new HashSet<CmsCulture>();
            Indices = new HashSet<CmsSearchIndex>();
            Resources = new HashSet<CmsResource>();
        }

        [Key]
        [Column("SiteID")]
        public int SiteId { get; set; }
        [StringLength(100)]
        public string SiteName { get; set; } = null!;
        [StringLength(200)]
        public string SiteDisplayName { get; set; } = null!;
        public string? SiteDescription { get; set; }
        [StringLength(20)]
        public string SiteStatus { get; set; } = null!;
        [StringLength(400)]
        public string SiteDomainName { get; set; } = null!;
        [StringLength(50)]
        public string? SiteDefaultVisitorCulture { get; set; }
        [Column("SiteGUID")]
        public Guid SiteGuid { get; set; }
        public DateTime SiteLastModified { get; set; }

        [InverseProperty("CampaignSite")]
        public virtual ICollection<AnalyticsCampaign> AnalyticsCampaigns { get; set; }
        [InverseProperty("ExitPageSite")]
        public virtual ICollection<AnalyticsExitPage> AnalyticsExitPages { get; set; }
        [InverseProperty("StatisticsSite")]
        public virtual ICollection<AnalyticsStatistic> AnalyticsStatistics { get; set; }
        [InverseProperty("Aclsite")]
        public virtual ICollection<CmsAcl> CmsAcls { get; set; }
        [InverseProperty("AlternativeUrlSite")]
        public virtual ICollection<CmsAlternativeUrl> CmsAlternativeUrls { get; set; }
        [InverseProperty("AttachmentSite")]
        public virtual ICollection<CmsAttachmentHistory> CmsAttachmentHistories { get; set; }
        [InverseProperty("AttachmentSite")]
        public virtual ICollection<CmsAttachment> CmsAttachments { get; set; }
        [InverseProperty("StateSite")]
        public virtual ICollection<CmsAutomationState> CmsAutomationStates { get; set; }
        [InverseProperty("CategorySite")]
        public virtual ICollection<CmsCategory> CmsCategories { get; set; }
        [InverseProperty("ScopeSite")]
        public virtual ICollection<CmsDocumentTypeScope> CmsDocumentTypeScopes { get; set; }
        [InverseProperty("EmailTemplateSite")]
        public virtual ICollection<CmsEmailTemplate> CmsEmailTemplates { get; set; }
        [InverseProperty("FormSite")]
        public virtual ICollection<CmsForm> CmsForms { get; set; }
        [InverseProperty("MembershipSite")]
        public virtual ICollection<CmsMembership> CmsMemberships { get; set; }
        [InverseProperty("MetaFileSite")]
        public virtual ICollection<CmsMetaFile> CmsMetaFiles { get; set; }
        [InverseProperty("VersionObjectSite")]
        public virtual ICollection<CmsObjectVersionHistory> CmsObjectVersionHistories { get; set; }
        [InverseProperty("PageFormerUrlPathSite")]
        public virtual ICollection<CmsPageFormerUrlPath> CmsPageFormerUrlPaths { get; set; }
        [InverseProperty("PageTemplateConfigurationSite")]
        public virtual ICollection<CmsPageTemplateConfiguration> CmsPageTemplateConfigurations { get; set; }
        [InverseProperty("PageUrlPathSite")]
        public virtual ICollection<CmsPageUrlPath> CmsPageUrlPaths { get; set; }
        [InverseProperty("PersonalizationSite")]
        public virtual ICollection<CmsPersonalization> CmsPersonalizations { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<CmsRole> CmsRoles { get; set; }
        [InverseProperty("TaskSite")]
        public virtual ICollection<CmsScheduledTask> CmsScheduledTasks { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<CmsSettingsKey> CmsSettingsKeys { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<CmsSiteDomainAlias> CmsSiteDomainAliases { get; set; }
        [InverseProperty("TagGroupSite")]
        public virtual ICollection<CmsTagGroup> CmsTagGroups { get; set; }
        [InverseProperty("NodeLinkedNodeSite")]
        public virtual ICollection<CmsTree> CmsTreeNodeLinkedNodeSites { get; set; }
        [InverseProperty("NodeSite")]
        public virtual ICollection<CmsTree> CmsTreeNodeSites { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<CmsUserCulture> CmsUserCultures { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<CmsUserSite> CmsUserSites { get; set; }
        [InverseProperty("NodeSite")]
        public virtual ICollection<CmsVersionHistory> CmsVersionHistories { get; set; }
        [InverseProperty("ScopeSite")]
        public virtual ICollection<CmsWorkflowScope> CmsWorkflowScopes { get; set; }
        [InverseProperty("BrandSite")]
        public virtual ICollection<ComBrand> ComBrands { get; set; }
        [InverseProperty("CarrierSite")]
        public virtual ICollection<ComCarrier> ComCarriers { get; set; }
        [InverseProperty("CollectionSite")]
        public virtual ICollection<ComCollection> ComCollections { get; set; }
        [InverseProperty("CurrencySite")]
        public virtual ICollection<ComCurrency> ComCurrencies { get; set; }
        [InverseProperty("EventSite")]
        public virtual ICollection<ComCustomerCreditHistory> ComCustomerCreditHistories { get; set; }
        [InverseProperty("CustomerSite")]
        public virtual ICollection<ComCustomer> ComCustomers { get; set; }
        [InverseProperty("DepartmentSite")]
        public virtual ICollection<ComDepartment> ComDepartments { get; set; }
        [InverseProperty("DiscountSite")]
        public virtual ICollection<ComDiscount> ComDiscounts { get; set; }
        [InverseProperty("ExchangeTableSite")]
        public virtual ICollection<ComExchangeTable> ComExchangeTables { get; set; }
        [InverseProperty("GiftCardSite")]
        public virtual ICollection<ComGiftCard> ComGiftCards { get; set; }
        [InverseProperty("InternalStatusSite")]
        public virtual ICollection<ComInternalStatus> ComInternalStatuses { get; set; }
        [InverseProperty("ManufacturerSite")]
        public virtual ICollection<ComManufacturer> ComManufacturers { get; set; }
        [InverseProperty("MultiBuyDiscountSite")]
        public virtual ICollection<ComMultiBuyDiscount> ComMultiBuyDiscounts { get; set; }
        [InverseProperty("CategorySite")]
        public virtual ICollection<ComOptionCategory> ComOptionCategories { get; set; }
        [InverseProperty("StatusSite")]
        public virtual ICollection<ComOrderStatus> ComOrderStatuses { get; set; }
        [InverseProperty("OrderSite")]
        public virtual ICollection<ComOrder> ComOrders { get; set; }
        [InverseProperty("PaymentOptionSite")]
        public virtual ICollection<ComPaymentOption> ComPaymentOptions { get; set; }
        [InverseProperty("PublicStatusSite")]
        public virtual ICollection<ComPublicStatus> ComPublicStatuses { get; set; }
        [InverseProperty("ShippingOptionSite")]
        public virtual ICollection<ComShippingOption> ComShippingOptions { get; set; }
        [InverseProperty("ShoppingCartSite")]
        public virtual ICollection<ComShoppingCart> ComShoppingCarts { get; set; }
        [InverseProperty("Skusite")]
        public virtual ICollection<ComSku> ComSkus { get; set; }
        [InverseProperty("SupplierSite")]
        public virtual ICollection<ComSupplier> ComSuppliers { get; set; }
        [InverseProperty("TaxClassSite")]
        public virtual ICollection<ComTaxClass> ComTaxClasses { get; set; }
        [InverseProperty("Site")]
        public virtual ICollection<ComWishlist> ComWishlists { get; set; }
        [InverseProperty("ExportSite")]
        public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        [InverseProperty("TaskSite")]
        public virtual ICollection<ExportTask> ExportTasks { get; set; }
        [InverseProperty("TaskSite")]
        public virtual ICollection<IntegrationTask> IntegrationTasks { get; set; }
        [InverseProperty("FileSite")]
        public virtual ICollection<MediaFile> MediaFiles { get; set; }
        [InverseProperty("LibrarySite")]
        public virtual ICollection<MediaLibrary> MediaLibraries { get; set; }
        [InverseProperty("TemplateSite")]
        public virtual ICollection<NewsletterEmailTemplate> NewsletterEmailTemplates { get; set; }
        [InverseProperty("EmailWidgetSite")]
        public virtual ICollection<NewsletterEmailWidget> NewsletterEmailWidgets { get; set; }
        [InverseProperty("EmailSite")]
        public virtual ICollection<NewsletterEmail> NewsletterEmails { get; set; }
        [InverseProperty("IssueSite")]
        public virtual ICollection<NewsletterNewsletterIssue> NewsletterNewsletterIssues { get; set; }
        [InverseProperty("NewsletterSite")]
        public virtual ICollection<NewsletterNewsletter> NewsletterNewsletters { get; set; }
        [InverseProperty("SubscriberSite")]
        public virtual ICollection<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        [InverseProperty("AbtestSite")]
        public virtual ICollection<OmAbtest> OmAbtests { get; set; }
        [InverseProperty("TrackedWebsiteSite")]
        public virtual ICollection<OmTrackedWebsite> OmTrackedWebsites { get; set; }
        [InverseProperty("ServerSite")]
        public virtual ICollection<StagingServer> StagingServers { get; set; }
        [InverseProperty("TaskSite")]
        public virtual ICollection<StagingTask> StagingTasks { get; set; }

        [ForeignKey("SiteId")]
        [InverseProperty("Sites")]
        public virtual ICollection<CmsClass> Classes { get; set; }
        [ForeignKey("SiteId")]
        [InverseProperty("Sites")]
        public virtual ICollection<CmsCulture> Cultures { get; set; }
        [ForeignKey("IndexSiteId")]
        [InverseProperty("IndexSites")]
        public virtual ICollection<CmsSearchIndex> Indices { get; set; }
        [ForeignKey("SiteId")]
        [InverseProperty("Sites")]
        public virtual ICollection<CmsResource> Resources { get; set; }
    }
}
