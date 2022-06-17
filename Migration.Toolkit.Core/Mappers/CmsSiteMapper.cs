using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSiteMapper: IEntityMapper<KX13.Models.CmsSite, KXO.Models.CmsSite>
{
    private readonly ILogger<CmsSiteMapper> _logger;

    public CmsSiteMapper(
        ILogger<CmsSiteMapper> logger
        )
    {
        _logger = logger;
    }
    
    public IModelMappingResult<KXO.Models.CmsSite> Map(KX13.Models.CmsSite? source, KXO.Models.CmsSite? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSite>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsSite();
            newInstance = true;
        }
        // For site guid match is not required!
        // else if (source.SiteGuid != target.SiteGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSite>();
        // }
        
        // target.SiteId = source.SiteId;
        target.SiteName = source.SiteName;
        target.SiteDisplayName = source.SiteDisplayName;
        target.SiteDescription = source.SiteDescription;
        target.SiteStatus = source.SiteStatus;
        target.SiteDomainName = source.SitePresentationUrl;
        // target.SiteDomainName = source.SiteDomainName; // TODO tk: 2022-06-01 check
        target.SiteDefaultVisitorCulture = source.SiteDefaultVisitorCulture;
        // target.SiteGuid = source.SiteGuid; // TODO tk: 2022-05-26 do not rewrite, instead add siteguid to mapping
        target.SiteLastModified = source.SiteLastModified;

        // [InverseProperty("CampaignSite")]
        // public virtual ICollection<AnalyticsCampaign> AnalyticsCampaigns { get; set; }
        // [InverseProperty("ExitPageSite")]
        // public virtual ICollection<AnalyticsExitPage> AnalyticsExitPages { get; set; }
        // [InverseProperty("StatisticsSite")]
        // public virtual ICollection<AnalyticsStatistic> AnalyticsStatistics { get; set; }
        // [InverseProperty("Aclsite")]
        // public virtual ICollection<CmsAcl> CmsAcls { get; set; }
        // [InverseProperty("AlternativeUrlSite")]
        // public virtual ICollection<CmsAlternativeUrl> CmsAlternativeUrls { get; set; }
        // [InverseProperty("AttachmentSite")]
        // public virtual ICollection<CmsAttachmentHistory> CmsAttachmentHistories { get; set; }
        // [InverseProperty("AttachmentSite")]
        // public virtual ICollection<CmsAttachment> CmsAttachments { get; set; }
        // [InverseProperty("StateSite")]
        // public virtual ICollection<CmsAutomationState> CmsAutomationStates { get; set; }
        // [InverseProperty("CategorySite")]
        // public virtual ICollection<CmsCategory> CmsCategories { get; set; }
        // [InverseProperty("ScopeSite")]
        // public virtual ICollection<CmsDocumentTypeScope> CmsDocumentTypeScopes { get; set; }
        // [InverseProperty("EmailTemplateSite")]
        // public virtual ICollection<CmsEmailTemplate> CmsEmailTemplates { get; set; }
        // [InverseProperty("FormSite")]
        // public virtual ICollection<CmsForm> CmsForms { get; set; }
        // [InverseProperty("MembershipSite")]
        // public virtual ICollection<CmsMembership> CmsMemberships { get; set; }
        // [InverseProperty("MetaFileSite")]
        // public virtual ICollection<CmsMetaFile> CmsMetaFiles { get; set; }
        // [InverseProperty("VersionObjectSite")]
        // public virtual ICollection<CmsObjectVersionHistory> CmsObjectVersionHistories { get; set; }
        // [InverseProperty("PageFormerUrlPathSite")]
        // public virtual ICollection<CmsPageFormerUrlPath> CmsPageFormerUrlPaths { get; set; }
        // [InverseProperty("PageTemplateConfigurationSite")]
        // public virtual ICollection<CmsPageTemplateConfiguration> CmsPageTemplateConfigurations { get; set; }
        // [InverseProperty("PageUrlPathSite")]
        // public virtual ICollection<CmsPageUrlPath> CmsPageUrlPaths { get; set; }
        // [InverseProperty("PersonalizationSite")]
        // public virtual ICollection<CmsPersonalization> CmsPersonalizations { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<CmsRole> CmsRoles { get; set; }
        // [InverseProperty("TaskSite")]
        // public virtual ICollection<CmsScheduledTask> CmsScheduledTasks { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<CmsSettingsKey> CmsSettingsKeys { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<CmsSiteDomainAlias> CmsSiteDomainAliases { get; set; }
        // [InverseProperty("TagGroupSite")]
        // public virtual ICollection<CmsTagGroup> CmsTagGroups { get; set; }
        // [InverseProperty("NodeLinkedNodeSite")]
        // public virtual ICollection<CmsTree> CmsTreeNodeLinkedNodeSites { get; set; }
        // [InverseProperty("NodeSite")]
        // public virtual ICollection<CmsTree> CmsTreeNodeSites { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<CmsUserCulture> CmsUserCultures { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<CmsUserSite> CmsUserSites { get; set; }
        // [InverseProperty("NodeSite")]
        // public virtual ICollection<CmsVersionHistory> CmsVersionHistories { get; set; }
        // [InverseProperty("ScopeSite")]
        // public virtual ICollection<CmsWorkflowScope> CmsWorkflowScopes { get; set; }
        // [InverseProperty("BrandSite")]
        // public virtual ICollection<ComBrand> ComBrands { get; set; }
        // [InverseProperty("CarrierSite")]
        // public virtual ICollection<ComCarrier> ComCarriers { get; set; }
        // [InverseProperty("CollectionSite")]
        // public virtual ICollection<ComCollection> ComCollections { get; set; }
        // [InverseProperty("CurrencySite")]
        // public virtual ICollection<ComCurrency> ComCurrencies { get; set; }
        // [InverseProperty("EventSite")]
        // public virtual ICollection<ComCustomerCreditHistory> ComCustomerCreditHistories { get; set; }
        // [InverseProperty("CustomerSite")]
        // public virtual ICollection<ComCustomer> ComCustomers { get; set; }
        // [InverseProperty("DepartmentSite")]
        // public virtual ICollection<ComDepartment> ComDepartments { get; set; }
        // [InverseProperty("DiscountSite")]
        // public virtual ICollection<ComDiscount> ComDiscounts { get; set; }
        // [InverseProperty("ExchangeTableSite")]
        // public virtual ICollection<ComExchangeTable> ComExchangeTables { get; set; }
        // [InverseProperty("GiftCardSite")]
        // public virtual ICollection<ComGiftCard> ComGiftCards { get; set; }
        // [InverseProperty("InternalStatusSite")]
        // public virtual ICollection<ComInternalStatus> ComInternalStatuses { get; set; }
        // [InverseProperty("ManufacturerSite")]
        // public virtual ICollection<ComManufacturer> ComManufacturers { get; set; }
        // [InverseProperty("MultiBuyDiscountSite")]
        // public virtual ICollection<ComMultiBuyDiscount> ComMultiBuyDiscounts { get; set; }
        // [InverseProperty("CategorySite")]
        // public virtual ICollection<ComOptionCategory> ComOptionCategories { get; set; }
        // [InverseProperty("StatusSite")]
        // public virtual ICollection<ComOrderStatus> ComOrderStatuses { get; set; }
        // [InverseProperty("OrderSite")]
        // public virtual ICollection<ComOrder> ComOrders { get; set; }
        // [InverseProperty("PaymentOptionSite")]
        // public virtual ICollection<ComPaymentOption> ComPaymentOptions { get; set; }
        // [InverseProperty("PublicStatusSite")]
        // public virtual ICollection<ComPublicStatus> ComPublicStatuses { get; set; }
        // [InverseProperty("ShippingOptionSite")]
        // public virtual ICollection<ComShippingOption> ComShippingOptions { get; set; }
        // [InverseProperty("ShoppingCartSite")]
        // public virtual ICollection<ComShoppingCart> ComShoppingCarts { get; set; }
        // [InverseProperty("Skusite")]
        // public virtual ICollection<ComSku> ComSkus { get; set; }
        // [InverseProperty("SupplierSite")]
        // public virtual ICollection<ComSupplier> ComSuppliers { get; set; }
        // [InverseProperty("TaxClassSite")]
        // public virtual ICollection<ComTaxClass> ComTaxClasses { get; set; }
        // [InverseProperty("Site")]
        // public virtual ICollection<ComWishlist> ComWishlists { get; set; }
        // [InverseProperty("ExportSite")]
        // public virtual ICollection<ExportHistory> ExportHistories { get; set; }
        // [InverseProperty("TaskSite")]
        // public virtual ICollection<ExportTask> ExportTasks { get; set; }
        // [InverseProperty("TaskSite")]
        // public virtual ICollection<IntegrationTask> IntegrationTasks { get; set; }
        // [InverseProperty("FileSite")]
        // public virtual ICollection<MediaFile> MediaFiles { get; set; }
        // [InverseProperty("LibrarySite")]
        // public virtual ICollection<MediaLibrary> MediaLibraries { get; set; }
        // [InverseProperty("TemplateSite")]
        // public virtual ICollection<NewsletterEmailTemplate> NewsletterEmailTemplates { get; set; }
        // [InverseProperty("EmailWidgetSite")]
        // public virtual ICollection<NewsletterEmailWidget> NewsletterEmailWidgets { get; set; }
        // [InverseProperty("EmailSite")]
        // public virtual ICollection<NewsletterEmail> NewsletterEmails { get; set; }
        // [InverseProperty("IssueSite")]
        // public virtual ICollection<NewsletterNewsletterIssue> NewsletterNewsletterIssues { get; set; }
        // [InverseProperty("NewsletterSite")]
        // public virtual ICollection<NewsletterNewsletter> NewsletterNewsletters { get; set; }
        // [InverseProperty("SubscriberSite")]
        // public virtual ICollection<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        // [InverseProperty("AbtestSite")]
        // public virtual ICollection<OmAbtest> OmAbtests { get; set; }
        // [InverseProperty("TrackedWebsiteSite")]
        // public virtual ICollection<OmTrackedWebsite> OmTrackedWebsites { get; set; }
        // [InverseProperty("ServerSite")]
        // public virtual ICollection<StagingServer> StagingServers { get; set; }
        // [InverseProperty("TaskSite")]
        // public virtual ICollection<StagingTask> StagingTasks { get; set; }

        // [ForeignKey("SiteId")]
        // [InverseProperty("Sites")]
        // public virtual ICollection<CmsClass> Classes { get; set; }
        // [ForeignKey("SiteId")]
        // [InverseProperty("Sites")]
        // public virtual ICollection<CmsWebPartContainer> Containers { get; set; }
        // [ForeignKey("SiteId")]
        // [InverseProperty("Sites")]
        // public virtual ICollection<CmsCulture> Cultures { get; set; }
        // [ForeignKey("IndexSiteId")]
        // [InverseProperty("IndexSites")]
        // public virtual ICollection<CmsSearchIndex> Indices { get; set; }
        // [ForeignKey("SiteId")]
        // [InverseProperty("Sites")]
        // public virtual ICollection<CmsRelationshipName> RelationshipNames { get; set; }
        // [ForeignKey("SiteId")]
        // [InverseProperty("Sites")]
        // public virtual ICollection<CmsResource> Resources { get; set; }
        
        return new ModelMappingSuccess<KXO.Models.CmsSite>(target, newInstance).Log(_logger);
    }
}