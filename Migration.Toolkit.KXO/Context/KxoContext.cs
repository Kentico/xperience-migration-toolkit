using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.KXO.Context
{
    public partial class KxoContext : DbContext
    {
        public KxoContext()
        {
        }

        public KxoContext(DbContextOptions<KxoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CdMigration> CdMigrations { get; set; } = null!;
        public virtual DbSet<CiFileMetadatum> CiFileMetadata { get; set; } = null!;
        public virtual DbSet<CiMigration> CiMigrations { get; set; } = null!;
        public virtual DbSet<CmsAlternativeForm> CmsAlternativeForms { get; set; } = null!;
        public virtual DbSet<CmsAlternativeUrl> CmsAlternativeUrls { get; set; } = null!;
        public virtual DbSet<CmsAutomationHistory> CmsAutomationHistories { get; set; } = null!;
        public virtual DbSet<CmsAutomationState> CmsAutomationStates { get; set; } = null!;
        public virtual DbSet<CmsAutomationTemplate> CmsAutomationTemplates { get; set; } = null!;
        public virtual DbSet<CmsClass> CmsClasses { get; set; } = null!;
        public virtual DbSet<CmsConsent> CmsConsents { get; set; } = null!;
        public virtual DbSet<CmsConsentAgreement> CmsConsentAgreements { get; set; } = null!;
        public virtual DbSet<CmsConsentArchive> CmsConsentArchives { get; set; } = null!;
        public virtual DbSet<CmsCountry> CmsCountries { get; set; } = null!;
        public virtual DbSet<CmsCulture> CmsCultures { get; set; } = null!;
        public virtual DbSet<CmsDocument> CmsDocuments { get; set; } = null!;
        public virtual DbSet<CmsEmail> CmsEmails { get; set; } = null!;
        public virtual DbSet<CmsEmailAttachment> CmsEmailAttachments { get; set; } = null!;
        public virtual DbSet<CmsEmailTemplate> CmsEmailTemplates { get; set; } = null!;
        public virtual DbSet<CmsEventLog> CmsEventLogs { get; set; } = null!;
        public virtual DbSet<CmsExternalLogin> CmsExternalLogins { get; set; } = null!;
        public virtual DbSet<CmsForm> CmsForms { get; set; } = null!;
        public virtual DbSet<CmsFormFeaturedField> CmsFormFeaturedFields { get; set; } = null!;
        public virtual DbSet<CmsLicenseKey> CmsLicenseKeys { get; set; } = null!;
        public virtual DbSet<CmsMacroIdentity> CmsMacroIdentities { get; set; } = null!;
        public virtual DbSet<CmsMacroRule> CmsMacroRules { get; set; } = null!;
        public virtual DbSet<CmsMacroRuleCategory> CmsMacroRuleCategories { get; set; } = null!;
        public virtual DbSet<CmsMacroRuleMacroRuleCategory> CmsMacroRuleMacroRuleCategories { get; set; } = null!;
        public virtual DbSet<CmsMembership> CmsMemberships { get; set; } = null!;
        public virtual DbSet<CmsMembershipUser> CmsMembershipUsers { get; set; } = null!;
        public virtual DbSet<CmsMetaFile> CmsMetaFiles { get; set; } = null!;
        public virtual DbSet<CmsObjectSetting> CmsObjectSettings { get; set; } = null!;
        public virtual DbSet<CmsObjectVersionHistory> CmsObjectVersionHistories { get; set; } = null!;
        public virtual DbSet<CmsObjectWorkflowTrigger> CmsObjectWorkflowTriggers { get; set; } = null!;
        public virtual DbSet<CmsPageFormerUrlPath> CmsPageFormerUrlPaths { get; set; } = null!;
        public virtual DbSet<CmsPageTemplateConfiguration> CmsPageTemplateConfigurations { get; set; } = null!;
        public virtual DbSet<CmsPageUrlPath> CmsPageUrlPaths { get; set; } = null!;
        public virtual DbSet<CmsPermission> CmsPermissions { get; set; } = null!;
        public virtual DbSet<CmsPersonalization> CmsPersonalizations { get; set; } = null!;
        public virtual DbSet<CmsQuery> CmsQueries { get; set; } = null!;
        public virtual DbSet<CmsResource> CmsResources { get; set; } = null!;
        public virtual DbSet<CmsResourceLibrary> CmsResourceLibraries { get; set; } = null!;
        public virtual DbSet<CmsResourceString> CmsResourceStrings { get; set; } = null!;
        public virtual DbSet<CmsResourceTranslation> CmsResourceTranslations { get; set; } = null!;
        public virtual DbSet<CmsRole> CmsRoles { get; set; } = null!;
        public virtual DbSet<CmsScheduledTask> CmsScheduledTasks { get; set; } = null!;
        public virtual DbSet<CmsSearchIndex> CmsSearchIndices { get; set; } = null!;
        public virtual DbSet<CmsSearchTask> CmsSearchTasks { get; set; } = null!;
        public virtual DbSet<CmsSearchTaskAzure> CmsSearchTaskAzures { get; set; } = null!;
        public virtual DbSet<CmsSettingsCategory> CmsSettingsCategories { get; set; } = null!;
        public virtual DbSet<CmsSettingsKey> CmsSettingsKeys { get; set; } = null!;
        public virtual DbSet<CmsSite> CmsSites { get; set; } = null!;
        public virtual DbSet<CmsSiteDomainAlias> CmsSiteDomainAliases { get; set; } = null!;
        public virtual DbSet<CmsState> CmsStates { get; set; } = null!;
        public virtual DbSet<CmsTimeZone> CmsTimeZones { get; set; } = null!;
        public virtual DbSet<CmsTransformation> CmsTransformations { get; set; } = null!;
        public virtual DbSet<CmsTree> CmsTrees { get; set; } = null!;
        public virtual DbSet<CmsUser> CmsUsers { get; set; } = null!;
        public virtual DbSet<CmsUserCulture> CmsUserCultures { get; set; } = null!;
        public virtual DbSet<CmsUserMacroIdentity> CmsUserMacroIdentities { get; set; } = null!;
        public virtual DbSet<CmsUserRole> CmsUserRoles { get; set; } = null!;
        public virtual DbSet<CmsUserSite> CmsUserSites { get; set; } = null!;
        public virtual DbSet<CmsVersionHistory> CmsVersionHistories { get; set; } = null!;
        public virtual DbSet<CmsWebFarmServer> CmsWebFarmServers { get; set; } = null!;
        public virtual DbSet<CmsWebFarmServerLog> CmsWebFarmServerLogs { get; set; } = null!;
        public virtual DbSet<CmsWebFarmServerMonitoring> CmsWebFarmServerMonitorings { get; set; } = null!;
        public virtual DbSet<CmsWebFarmServerTask> CmsWebFarmServerTasks { get; set; } = null!;
        public virtual DbSet<CmsWebFarmTask> CmsWebFarmTasks { get; set; } = null!;
        public virtual DbSet<CmsWorkflow> CmsWorkflows { get; set; } = null!;
        public virtual DbSet<CmsWorkflowAction> CmsWorkflowActions { get; set; } = null!;
        public virtual DbSet<CmsWorkflowHistory> CmsWorkflowHistories { get; set; } = null!;
        public virtual DbSet<CmsWorkflowScope> CmsWorkflowScopes { get; set; } = null!;
        public virtual DbSet<CmsWorkflowStep> CmsWorkflowSteps { get; set; } = null!;
        public virtual DbSet<CmsWorkflowStepRole> CmsWorkflowStepRoles { get; set; } = null!;
        public virtual DbSet<CmsWorkflowStepUser> CmsWorkflowStepUsers { get; set; } = null!;
        public virtual DbSet<CmsWorkflowTransition> CmsWorkflowTransitions { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreAboutUsSection> DancingGoatCoreAboutUsSections { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreArticle> DancingGoatCoreArticles { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreCafe> DancingGoatCoreCaves { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreCoffee> DancingGoatCoreCoffees { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreContact> DancingGoatCoreContacts { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreHomeSection> DancingGoatCoreHomeSections { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreNavigationItem> DancingGoatCoreNavigationItems { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreReference> DancingGoatCoreReferences { get; set; } = null!;
        public virtual DbSet<DancingGoatCoreSocialLink> DancingGoatCoreSocialLinks { get; set; } = null!;
        public virtual DbSet<ExportHistory> ExportHistories { get; set; } = null!;
        public virtual DbSet<ExportTask> ExportTasks { get; set; } = null!;
        public virtual DbSet<FormDancingGoatCoreCoffeeSampleList> FormDancingGoatCoreCoffeeSampleLists { get; set; } = null!;
        public virtual DbSet<FormDancingGoatCoreContactUsNew> FormDancingGoatCoreContactUsNews { get; set; } = null!;
        public virtual DbSet<IntegrationConnector> IntegrationConnectors { get; set; } = null!;
        public virtual DbSet<IntegrationSyncLog> IntegrationSyncLogs { get; set; } = null!;
        public virtual DbSet<IntegrationSynchronization> IntegrationSynchronizations { get; set; } = null!;
        public virtual DbSet<IntegrationTask> IntegrationTasks { get; set; } = null!;
        public virtual DbSet<MediaFile> MediaFiles { get; set; } = null!;
        public virtual DbSet<MediaLibrary> MediaLibraries { get; set; } = null!;
        public virtual DbSet<MediaLibraryRolePermission> MediaLibraryRolePermissions { get; set; } = null!;
        public virtual DbSet<OmAbtest> OmAbtests { get; set; } = null!;
        public virtual DbSet<OmAbvariantDatum> OmAbvariantData { get; set; } = null!;
        public virtual DbSet<OmAccount> OmAccounts { get; set; } = null!;
        public virtual DbSet<OmAccountContact> OmAccountContacts { get; set; } = null!;
        public virtual DbSet<OmAccountStatus> OmAccountStatuses { get; set; } = null!;
        public virtual DbSet<OmActivity> OmActivities { get; set; } = null!;
        public virtual DbSet<OmActivityRecalculationQueue> OmActivityRecalculationQueues { get; set; } = null!;
        public virtual DbSet<OmActivityType> OmActivityTypes { get; set; } = null!;
        public virtual DbSet<OmContact> OmContacts { get; set; } = null!;
        public virtual DbSet<OmContactChangeRecalculationQueue> OmContactChangeRecalculationQueues { get; set; } = null!;
        public virtual DbSet<OmContactGroup> OmContactGroups { get; set; } = null!;
        public virtual DbSet<OmContactGroupMember> OmContactGroupMembers { get; set; } = null!;
        public virtual DbSet<OmContactRole> OmContactRoles { get; set; } = null!;
        public virtual DbSet<OmContactStatus> OmContactStatuses { get; set; } = null!;
        public virtual DbSet<OmMembership> OmMemberships { get; set; } = null!;
        public virtual DbSet<OmTrackedWebsite> OmTrackedWebsites { get; set; } = null!;
        public virtual DbSet<OmVisitorToContact> OmVisitorToContacts { get; set; } = null!;
        public virtual DbSet<StagingServer> StagingServers { get; set; } = null!;
        public virtual DbSet<StagingSynchronization> StagingSynchronizations { get; set; } = null!;
        public virtual DbSet<StagingTask> StagingTasks { get; set; } = null!;
        public virtual DbSet<StagingTaskGroup> StagingTaskGroups { get; set; } = null!;
        public virtual DbSet<StagingTaskGroupTask> StagingTaskGroupTasks { get; set; } = null!;
        public virtual DbSet<StagingTaskGroupUser> StagingTaskGroupUsers { get; set; } = null!;
        public virtual DbSet<StagingTaskUser> StagingTaskUsers { get; set; } = null!;
        public virtual DbSet<TempFile> TempFiles { get; set; } = null!;
        public virtual DbSet<TempPageBuilderWidget> TempPageBuilderWidgets { get; set; } = null!;
        public virtual DbSet<ViewCmsObjectVersionHistoryUserJoined> ViewCmsObjectVersionHistoryUserJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsResourceStringJoined> ViewCmsResourceStringJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsResourceTranslatedJoined> ViewCmsResourceTranslatedJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsRoleResourcePermissionJoined> ViewCmsRoleResourcePermissionJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsSiteDocumentCount> ViewCmsSiteDocumentCounts { get; set; } = null!;
        public virtual DbSet<ViewCmsTreeJoined> ViewCmsTreeJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsUserDocument> ViewCmsUserDocuments { get; set; } = null!;
        public virtual DbSet<ViewCmsUserRoleJoined> ViewCmsUserRoleJoineds { get; set; } = null!;
        public virtual DbSet<ViewCmsUserRoleMembershipRole> ViewCmsUserRoleMembershipRoles { get; set; } = null!;
        public virtual DbSet<ViewCmsUserRoleMembershipRoleValidOnlyJoined> ViewCmsUserRoleMembershipRoleValidOnlyJoineds { get; set; } = null!;
        public virtual DbSet<ViewIntegrationTaskJoined> ViewIntegrationTaskJoineds { get; set; } = null!;
        public virtual DbSet<ViewMembershipMembershipUserJoined> ViewMembershipMembershipUserJoineds { get; set; } = null!;
        public virtual DbSet<ViewOmAccountContactAccountJoined> ViewOmAccountContactAccountJoineds { get; set; } = null!;
        public virtual DbSet<ViewOmAccountContactContactJoined> ViewOmAccountContactContactJoineds { get; set; } = null!;
        public virtual DbSet<ViewOmAccountJoined> ViewOmAccountJoineds { get; set; } = null!;
        public virtual DbSet<ViewOmContactGroupMemberAccountJoined> ViewOmContactGroupMemberAccountJoineds { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CdMigration>(entity =>
            {
                entity.Property(e => e.DateApplied).HasDefaultValueSql("(sysdatetime())");
            });

            modelBuilder.Entity<CiFileMetadatum>(entity =>
            {
                entity.Property(e => e.FileHash).HasDefaultValueSql("(N'')");

                entity.Property(e => e.FileLocation).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CiMigration>(entity =>
            {
                entity.Property(e => e.DateApplied).HasDefaultValueSql("(sysdatetime())");
            });

            modelBuilder.Entity<CmsAlternativeForm>(entity =>
            {
                entity.Property(e => e.FormDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.FormHideNewParentFields).HasDefaultValueSql("((0))");

                entity.Property(e => e.FormIsCustom).HasDefaultValueSql("((0))");

                entity.Property(e => e.FormName).HasDefaultValueSql("('')");

                entity.HasOne(d => d.FormClass)
                    .WithMany(p => p.CmsAlternativeFormFormClasses)
                    .HasForeignKey(d => d.FormClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AlternativeForm_FormClassID_CMS_Class");

                entity.HasOne(d => d.FormCoupledClass)
                    .WithMany(p => p.CmsAlternativeFormFormCoupledClasses)
                    .HasForeignKey(d => d.FormCoupledClassId)
                    .HasConstraintName("FK_CMS_AlternativeForm_FormCoupledClassID_CMS_Class");
            });

            modelBuilder.Entity<CmsAlternativeUrl>(entity =>
            {
                entity.Property(e => e.AlternativeUrlLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.AlternativeUrlUrl).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.AlternativeUrlDocument)
                    .WithMany(p => p.CmsAlternativeUrls)
                    .HasForeignKey(d => d.AlternativeUrlDocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AlternativeUrl_CMS_Document");

                entity.HasOne(d => d.AlternativeUrlSite)
                    .WithMany(p => p.CmsAlternativeUrls)
                    .HasForeignKey(d => d.AlternativeUrlSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AlternativeUrl_CMS_Site");
            });

            modelBuilder.Entity<CmsAutomationHistory>(entity =>
            {
                entity.Property(e => e.HistoryRejected).HasDefaultValueSql("((0))");

                entity.Property(e => e.HistoryStepDisplayName).HasDefaultValueSql("('')");

                entity.HasOne(d => d.HistoryApprovedByUser)
                    .WithMany(p => p.CmsAutomationHistories)
                    .HasForeignKey(d => d.HistoryApprovedByUserId)
                    .HasConstraintName("FK_CMS_AutomationHistory_HistoryApprovedByUserID");

                entity.HasOne(d => d.HistoryState)
                    .WithMany(p => p.CmsAutomationHistories)
                    .HasForeignKey(d => d.HistoryStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AutomationHistory_HistoryStateID");

                entity.HasOne(d => d.HistoryStep)
                    .WithMany(p => p.CmsAutomationHistoryHistorySteps)
                    .HasForeignKey(d => d.HistoryStepId)
                    .HasConstraintName("FK_CMS_AutomationHistory_HistoryStepID");

                entity.HasOne(d => d.HistoryTargetStep)
                    .WithMany(p => p.CmsAutomationHistoryHistoryTargetSteps)
                    .HasForeignKey(d => d.HistoryTargetStepId)
                    .HasConstraintName("FK_CMS_AutomationHistory_HistoryTargetStepID");

                entity.HasOne(d => d.HistoryWorkflow)
                    .WithMany(p => p.CmsAutomationHistories)
                    .HasForeignKey(d => d.HistoryWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AutomationHistory_HistoryWorkflowID");
            });

            modelBuilder.Entity<CmsAutomationState>(entity =>
            {
                entity.HasOne(d => d.StateSite)
                    .WithMany(p => p.CmsAutomationStates)
                    .HasForeignKey(d => d.StateSiteId)
                    .HasConstraintName("FK_CMS_AutomationState_StateSiteID_CMS_Site");

                entity.HasOne(d => d.StateStep)
                    .WithMany(p => p.CmsAutomationStates)
                    .HasForeignKey(d => d.StateStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AutomationState_StateStepID");

                entity.HasOne(d => d.StateUser)
                    .WithMany(p => p.CmsAutomationStates)
                    .HasForeignKey(d => d.StateUserId)
                    .HasConstraintName("FK_CMS_AutomationState_StateUserID_CMS_User");

                entity.HasOne(d => d.StateWorkflow)
                    .WithMany(p => p.CmsAutomationStates)
                    .HasForeignKey(d => d.StateWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_AutomationState_StateWorkflowID");
            });

            modelBuilder.Entity<CmsAutomationTemplate>(entity =>
            {
                entity.Property(e => e.TemplateDisplayName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TemplateLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");
            });

            modelBuilder.Entity<CmsClass>(entity =>
            {
                entity.HasKey(e => e.ClassId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.ClassId, e.ClassName, e.ClassDisplayName }, "IX_CMS_Class_ClassID_ClassName_ClassDisplayName")
                    .IsClustered();

                entity.HasOne(d => d.ClassResource)
                    .WithMany(p => p.CmsClasses)
                    .HasForeignKey(d => d.ClassResourceId)
                    .HasConstraintName("FK_CMS_Class_ClassResourceID_CMS_Resource");

                entity.HasMany(d => d.Sites)
                    .WithMany(p => p.Classes)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsClassSite",
                        l => l.HasOne<CmsSite>().WithMany().HasForeignKey("SiteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_Class_SiteID_CMS_Site"),
                        r => r.HasOne<CmsClass>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_Class_ClassID_CMS_Class"),
                        j =>
                        {
                            j.HasKey("ClassId", "SiteId");

                            j.ToTable("CMS_ClassSite");

                            j.HasIndex(new[] { "SiteId" }, "IX_CMS_ClassSite_SiteID");

                            j.IndexerProperty<int>("ClassId").HasColumnName("ClassID");

                            j.IndexerProperty<int>("SiteId").HasColumnName("SiteID");
                        });
            });

            modelBuilder.Entity<CmsConsent>(entity =>
            {
                entity.Property(e => e.ConsentContent).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ConsentDisplayName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ConsentHash).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ConsentLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.ConsentName).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsConsentAgreement>(entity =>
            {
                entity.Property(e => e.ConsentAgreementTime).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.HasOne(d => d.ConsentAgreementConsent)
                    .WithMany(p => p.CmsConsentAgreements)
                    .HasForeignKey(d => d.ConsentAgreementConsentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ConsentAgreement_ConsentAgreementConsentID_CMS_Consent");

                entity.HasOne(d => d.ConsentAgreementContact)
                    .WithMany(p => p.CmsConsentAgreements)
                    .HasForeignKey(d => d.ConsentAgreementContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ConsentAgreement_ConsentAgreementContactID_OM_Contact");
            });

            modelBuilder.Entity<CmsConsentArchive>(entity =>
            {
                entity.Property(e => e.ConsentArchiveContent).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ConsentArchiveHash).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ConsentArchiveLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.HasOne(d => d.ConsentArchiveConsent)
                    .WithMany(p => p.CmsConsentArchives)
                    .HasForeignKey(d => d.ConsentArchiveConsentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ConsentArchive_ConsentArchiveConsentID_CMS_Consent");
            });

            modelBuilder.Entity<CmsCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CountryDisplayName, "IX_CMS_Country_CountryDisplayName")
                    .IsClustered();

                entity.Property(e => e.CountryDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.CountryLastModified).HasDefaultValueSql("('11/14/2013 1:43:04 PM')");

                entity.Property(e => e.CountryName).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CmsCulture>(entity =>
            {
                entity.HasKey(e => e.CultureId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CultureName, "IX_CMS_Culture_CultureName")
                    .IsClustered();

                entity.Property(e => e.CultureIsUiculture).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CmsDocument>(entity =>
            {
                entity.Property(e => e.DocumentCanBePublished).HasDefaultValueSql("((1))");

                entity.Property(e => e.DocumentCulture).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.DocumentCheckedOutByUser)
                    .WithMany(p => p.CmsDocumentDocumentCheckedOutByUsers)
                    .HasForeignKey(d => d.DocumentCheckedOutByUserId)
                    .HasConstraintName("FK_CMS_Document_DocumentCheckedOutByUserID_CMS_User");

                entity.HasOne(d => d.DocumentCheckedOutVersionHistory)
                    .WithMany(p => p.CmsDocumentDocumentCheckedOutVersionHistories)
                    .HasForeignKey(d => d.DocumentCheckedOutVersionHistoryId)
                    .HasConstraintName("FK_CMS_Document_DocumentCheckedOutVersionHistoryID_CMS_VersionHistory");

                entity.HasOne(d => d.DocumentCreatedByUser)
                    .WithMany(p => p.CmsDocumentDocumentCreatedByUsers)
                    .HasForeignKey(d => d.DocumentCreatedByUserId)
                    .HasConstraintName("FK_CMS_Document_DocumentCreatedByUserID_CMS_User");

                entity.HasOne(d => d.DocumentModifiedByUser)
                    .WithMany(p => p.CmsDocumentDocumentModifiedByUsers)
                    .HasForeignKey(d => d.DocumentModifiedByUserId)
                    .HasConstraintName("FK_CMS_Document_DocumentModifiedByUserID_CMS_User");

                entity.HasOne(d => d.DocumentNode)
                    .WithMany(p => p.CmsDocuments)
                    .HasForeignKey(d => d.DocumentNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Document_DocumentNodeID_CMS_Tree");

                entity.HasOne(d => d.DocumentPublishedVersionHistory)
                    .WithMany(p => p.CmsDocumentDocumentPublishedVersionHistories)
                    .HasForeignKey(d => d.DocumentPublishedVersionHistoryId)
                    .HasConstraintName("FK_CMS_Document_DocumentPublishedVersionHistoryID_CMS_VersionHistory");

                entity.HasOne(d => d.DocumentWorkflowStep)
                    .WithMany(p => p.CmsDocuments)
                    .HasForeignKey(d => d.DocumentWorkflowStepId)
                    .HasConstraintName("FK_CMS_Document_DocumentWorkflowStepID_CMS_WorkflowStep");
            });

            modelBuilder.Entity<CmsEmail>(entity =>
            {
                entity.Property(e => e.EmailFrom).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EmailLastModified).HasDefaultValueSql("('6/17/2016 10:11:21 AM')");

                entity.Property(e => e.EmailSubject).HasDefaultValueSql("('')");

                entity.HasMany(d => d.Attachments)
                    .WithMany(p => p.Emails)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsAttachmentForEmail",
                        l => l.HasOne<CmsEmailAttachment>().WithMany().HasForeignKey("AttachmentId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_AttachmentForEmail_AttachmentID_CMS_EmailAttachment"),
                        r => r.HasOne<CmsEmail>().WithMany().HasForeignKey("EmailId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_AttachmentForEmail_EmailID_CMS_Email"),
                        j =>
                        {
                            j.HasKey("EmailId", "AttachmentId");

                            j.ToTable("CMS_AttachmentForEmail");

                            j.HasIndex(new[] { "AttachmentId" }, "IX_CMS_AttachmentForEmail_AttachmentID");

                            j.IndexerProperty<int>("EmailId").HasColumnName("EmailID");

                            j.IndexerProperty<int>("AttachmentId").HasColumnName("AttachmentID");
                        });
            });

            modelBuilder.Entity<CmsEmailTemplate>(entity =>
            {
                entity.HasKey(e => e.EmailTemplateId)
                    .IsClustered(false);

                entity.HasIndex(e => e.EmailTemplateDisplayName, "IX_CMS_EmailTemplate_EmailTemplateDisplayName")
                    .IsClustered();

                entity.Property(e => e.EmailTemplateDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.EmailTemplateName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.EmailTemplateSite)
                    .WithMany(p => p.CmsEmailTemplates)
                    .HasForeignKey(d => d.EmailTemplateSiteId)
                    .HasConstraintName("FK_CMS_Email_EmailTemplateSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsEventLog>(entity =>
            {
                entity.Property(e => e.DocumentName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EventCode).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EventMachineName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EventTime).HasDefaultValueSql("('4/21/2015 8:21:43 AM')");

                entity.Property(e => e.EventType).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EventUrl).HasDefaultValueSql("(N'')");

                entity.Property(e => e.EventUrlReferrer).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Ipaddress).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Source).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsExternalLogin>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsExternalLogins)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ExternalLogin_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsForm>(entity =>
            {
                entity.HasKey(e => e.FormId)
                    .IsClustered(false);

                entity.HasIndex(e => e.FormDisplayName, "IX_CMS_Form_FormDisplayName")
                    .IsClustered();

                entity.Property(e => e.FormAfterSubmitMode).HasDefaultValueSql("(N'')");

                entity.Property(e => e.FormDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.FormLastModified).HasDefaultValueSql("('9/17/2012 1:37:08 PM')");

                entity.Property(e => e.FormLogActivity).HasDefaultValueSql("((1))");

                entity.Property(e => e.FormName).HasDefaultValueSql("('')");

                entity.Property(e => e.FormSubmitButtonText).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.FormClass)
                    .WithMany(p => p.CmsForms)
                    .HasForeignKey(d => d.FormClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Form_FormClassID_CMS_Class");

                entity.HasOne(d => d.FormSite)
                    .WithMany(p => p.CmsForms)
                    .HasForeignKey(d => d.FormSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Form_FormSiteID_CMS_Site");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Forms)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsFormRole",
                        l => l.HasOne<CmsRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_FormRole_RoleID_CMS_Role"),
                        r => r.HasOne<CmsForm>().WithMany().HasForeignKey("FormId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_FormRole_FormID_CMS_Form"),
                        j =>
                        {
                            j.HasKey("FormId", "RoleId");

                            j.ToTable("CMS_FormRole");

                            j.HasIndex(new[] { "RoleId" }, "IX_CMS_FormRole_RoleID");

                            j.IndexerProperty<int>("FormId").HasColumnName("FormID");

                            j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                        });
            });

            modelBuilder.Entity<CmsFormFeaturedField>(entity =>
            {
                entity.Property(e => e.FormFeaturedFieldMapping).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsLicenseKey>(entity =>
            {
                entity.HasKey(e => e.LicenseKeyId)
                    .IsClustered(false);

                entity.HasIndex(e => e.LicenseDomain, "IX_CMS_LicenseKey_LicenseDomain")
                    .IsClustered();
            });

            modelBuilder.Entity<CmsMacroIdentity>(entity =>
            {
                entity.Property(e => e.MacroIdentityLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.MacroIdentityName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.MacroIdentityEffectiveUser)
                    .WithMany(p => p.CmsMacroIdentities)
                    .HasForeignKey(d => d.MacroIdentityEffectiveUserId)
                    .HasConstraintName("FK_CMS_MacroIdentity_MacroIdentityEffectiveUserID_CMS_User");
            });

            modelBuilder.Entity<CmsMacroRule>(entity =>
            {
                entity.Property(e => e.MacroRuleAvailability).HasDefaultValueSql("((0))");

                entity.Property(e => e.MacroRuleCondition).HasDefaultValueSql("(N'')");

                entity.Property(e => e.MacroRuleDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.MacroRuleEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.MacroRuleIsCustom).HasDefaultValueSql("((0))");

                entity.Property(e => e.MacroRuleLastModified).HasDefaultValueSql("('5/1/2012 8:46:33 AM')");
            });

            modelBuilder.Entity<CmsMacroRuleMacroRuleCategory>(entity =>
            {
                entity.HasOne(d => d.MacroRuleCategory)
                    .WithMany(p => p.CmsMacroRuleMacroRuleCategories)
                    .HasForeignKey(d => d.MacroRuleCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_MacroRuleMacroRuleCategory_MacroRuleCategoryID_CMS_MacroRuleMacroRuleCategory");

                entity.HasOne(d => d.MacroRule)
                    .WithMany(p => p.CmsMacroRuleMacroRuleCategories)
                    .HasForeignKey(d => d.MacroRuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_MacroRuleMacroRuleCategory_MacroRuleID_CMS_MacroRule");
            });

            modelBuilder.Entity<CmsMembership>(entity =>
            {
                entity.HasOne(d => d.MembershipSite)
                    .WithMany(p => p.CmsMemberships)
                    .HasForeignKey(d => d.MembershipSiteId)
                    .HasConstraintName("FK_CMS_Membership_MembershipSiteID_CMS_Site");

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Memberships)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsMembershipRole",
                        l => l.HasOne<CmsRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_MembershipRole_RoleID_CMS_Role"),
                        r => r.HasOne<CmsMembership>().WithMany().HasForeignKey("MembershipId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_MembershipRole_MembershipID_CMS_Membership"),
                        j =>
                        {
                            j.HasKey("MembershipId", "RoleId");

                            j.ToTable("CMS_MembershipRole");

                            j.HasIndex(new[] { "RoleId" }, "IX_CMS_MembershipRole_RoleID");

                            j.IndexerProperty<int>("MembershipId").HasColumnName("MembershipID");

                            j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                        });
            });

            modelBuilder.Entity<CmsMembershipUser>(entity =>
            {
                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.CmsMembershipUsers)
                    .HasForeignKey(d => d.MembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_MembershipUser_MembershipID_CMS_Membership");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsMembershipUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_MembershipUser_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsMetaFile>(entity =>
            {
                entity.HasKey(e => e.MetaFileId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.MetaFileObjectType, e.MetaFileObjectId, e.MetaFileGroupName }, "IX_CMS_Metafile_MetaFileObjectType_MetaFileObjectID_MetaFileGroupName")
                    .IsClustered();

                entity.HasOne(d => d.MetaFileSite)
                    .WithMany(p => p.CmsMetaFiles)
                    .HasForeignKey(d => d.MetaFileSiteId)
                    .HasConstraintName("FK_CMS_MetaFile_MetaFileSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsObjectSetting>(entity =>
            {
                entity.Property(e => e.ObjectSettingsObjectType).HasDefaultValueSql("('')");

                entity.Property(e => e.ObjectWorkflowSendEmails).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ObjectCheckedOutByUser)
                    .WithMany(p => p.CmsObjectSettings)
                    .HasForeignKey(d => d.ObjectCheckedOutByUserId)
                    .HasConstraintName("FK_CMS_ObjectSettings_ObjectCheckedOutByUserID_CMS_User");

                entity.HasOne(d => d.ObjectCheckedOutVersionHistory)
                    .WithMany(p => p.CmsObjectSettingObjectCheckedOutVersionHistories)
                    .HasForeignKey(d => d.ObjectCheckedOutVersionHistoryId)
                    .HasConstraintName("FK_CMS_ObjectSettings_ObjectCheckedOutVersionHistoryID_CMS_ObjectVersionHistory");

                entity.HasOne(d => d.ObjectPublishedVersionHistory)
                    .WithMany(p => p.CmsObjectSettingObjectPublishedVersionHistories)
                    .HasForeignKey(d => d.ObjectPublishedVersionHistoryId)
                    .HasConstraintName("FK_CMS_ObjectSettings_ObjectPublishedVersionHistoryID_CMS_ObjectVersionHistory");

                entity.HasOne(d => d.ObjectWorkflowStep)
                    .WithMany(p => p.CmsObjectSettings)
                    .HasForeignKey(d => d.ObjectWorkflowStepId)
                    .HasConstraintName("FK_CMS_ObjectSettings_ObjectWorkflowStepID_CMS_WorkflowStep");
            });

            modelBuilder.Entity<CmsObjectVersionHistory>(entity =>
            {
                entity.HasKey(e => e.VersionId)
                    .HasName("PK_CMS_ObjectVersionHistory_VersionID")
                    .IsClustered(false);

                entity.HasIndex(e => new { e.VersionObjectType, e.VersionObjectId, e.VersionId }, "PK_CMS_ObjectVersionHistory")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.VersionNumber).HasDefaultValueSql("('')");

                entity.HasOne(d => d.VersionDeletedByUser)
                    .WithMany(p => p.CmsObjectVersionHistoryVersionDeletedByUsers)
                    .HasForeignKey(d => d.VersionDeletedByUserId)
                    .HasConstraintName("FK_CMS_ObjectVersionHistory_VersionDeletedByUserID_CMS_User");

                entity.HasOne(d => d.VersionModifiedByUser)
                    .WithMany(p => p.CmsObjectVersionHistoryVersionModifiedByUsers)
                    .HasForeignKey(d => d.VersionModifiedByUserId)
                    .HasConstraintName("FK_CMS_ObjectVersionHistory_VersionModifiedByUserID_CMS_User");

                entity.HasOne(d => d.VersionObjectSite)
                    .WithMany(p => p.CmsObjectVersionHistories)
                    .HasForeignKey(d => d.VersionObjectSiteId)
                    .HasConstraintName("FK_CMS_ObjectVersionHistory_VersionObjectSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsObjectWorkflowTrigger>(entity =>
            {
                entity.Property(e => e.TriggerDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.TriggerObjectType).HasDefaultValueSql("('')");

                entity.HasOne(d => d.TriggerWorkflow)
                    .WithMany(p => p.CmsObjectWorkflowTriggers)
                    .HasForeignKey(d => d.TriggerWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ObjectWorkflowTrigger_TriggerWorkflowID");
            });

            modelBuilder.Entity<CmsPageFormerUrlPath>(entity =>
            {
                entity.Property(e => e.PageFormerUrlPathCulture).HasDefaultValueSql("(N'')");

                entity.Property(e => e.PageFormerUrlPathLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.PageFormerUrlPathUrlPath).HasDefaultValueSql("(N'')");

                entity.Property(e => e.PageFormerUrlPathUrlPathHash).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.PageFormerUrlPathNode)
                    .WithMany(p => p.CmsPageFormerUrlPaths)
                    .HasForeignKey(d => d.PageFormerUrlPathNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_PageFormerUrlPath_PageFormerUrlPathNodeID_CMS_Tree");

                entity.HasOne(d => d.PageFormerUrlPathSite)
                    .WithMany(p => p.CmsPageFormerUrlPaths)
                    .HasForeignKey(d => d.PageFormerUrlPathSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_PageFormerUrlPath_PageFormerUrlPathSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsPageTemplateConfiguration>(entity =>
            {
                entity.Property(e => e.PageTemplateConfigurationLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.PageTemplateConfigurationName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.PageTemplateConfigurationTemplate).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.PageTemplateConfigurationSite)
                    .WithMany(p => p.CmsPageTemplateConfigurations)
                    .HasForeignKey(d => d.PageTemplateConfigurationSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_PageTemplateConfiguration_PageTemplateConfigurationSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsPageUrlPath>(entity =>
            {
                entity.Property(e => e.PageUrlPathCulture).HasDefaultValueSql("(N'')");

                entity.Property(e => e.PageUrlPathLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.PageUrlPathUrlPath).HasDefaultValueSql("(N'')");

                entity.Property(e => e.PageUrlPathUrlPathHash).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.PageUrlPathNode)
                    .WithMany(p => p.CmsPageUrlPaths)
                    .HasForeignKey(d => d.PageUrlPathNodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_PageUrlPath_PageUrlPathNodeID_CMS_Tree");

                entity.HasOne(d => d.PageUrlPathSite)
                    .WithMany(p => p.CmsPageUrlPaths)
                    .HasForeignKey(d => d.PageUrlPathSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_PageUrlPath_PageUrlPathSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsPermission>(entity =>
            {
                entity.Property(e => e.PermissionDisplayInMatrix).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CmsPermissions)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_CMS_Permission_ClassID_CMS_Class");

                entity.HasOne(d => d.Resource)
                    .WithMany(p => p.CmsPermissions)
                    .HasForeignKey(d => d.ResourceId)
                    .HasConstraintName("FK_CMS_Permission_ResourceID_CMS_Resource");
            });

            modelBuilder.Entity<CmsPersonalization>(entity =>
            {
                entity.Property(e => e.PersonalizationLastModified).HasDefaultValueSql("('9/2/2008 5:36:59 PM')");

                entity.HasOne(d => d.PersonalizationSite)
                    .WithMany(p => p.CmsPersonalizations)
                    .HasForeignKey(d => d.PersonalizationSiteId)
                    .HasConstraintName("FK_CMS_Personalization_PersonalizationSiteID_CMS_Site");

                entity.HasOne(d => d.PersonalizationUser)
                    .WithMany(p => p.CmsPersonalizations)
                    .HasForeignKey(d => d.PersonalizationUserId)
                    .HasConstraintName("FK_CMS_Personalization_PersonalizationUserID_CMS_User");
            });

            modelBuilder.Entity<CmsQuery>(entity =>
            {
                entity.Property(e => e.QueryIsCustom).HasDefaultValueSql("((0))");

                entity.Property(e => e.QueryName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.CmsQueries)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_CMS_Query_ClassID_CMS_Class");
            });

            modelBuilder.Entity<CmsResource>(entity =>
            {
                entity.HasKey(e => e.ResourceId)
                    .IsClustered(false);

                entity.HasIndex(e => e.ResourceDisplayName, "IX_CMS_Resource_ResourceDisplayName")
                    .IsClustered();

                entity.Property(e => e.ResourceHasFiles).HasDefaultValueSql("((0))");

                entity.Property(e => e.ResourceInstallationState).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ResourceInstalledVersion).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ShowInDevelopment).HasDefaultValueSql("((0))");

                entity.HasMany(d => d.Sites)
                    .WithMany(p => p.Resources)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsResourceSite",
                        l => l.HasOne<CmsSite>().WithMany().HasForeignKey("SiteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_ResourceSite_SiteID_CMS_Site"),
                        r => r.HasOne<CmsResource>().WithMany().HasForeignKey("ResourceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_ResourceSite_ResourceID_CMS_Resource"),
                        j =>
                        {
                            j.HasKey("ResourceId", "SiteId");

                            j.ToTable("CMS_ResourceSite");

                            j.HasIndex(new[] { "SiteId" }, "IX_CMS_ResourceSite_SiteID");

                            j.IndexerProperty<int>("ResourceId").HasColumnName("ResourceID");

                            j.IndexerProperty<int>("SiteId").HasColumnName("SiteID");
                        });
            });

            modelBuilder.Entity<CmsResourceLibrary>(entity =>
            {
                entity.Property(e => e.ResourceLibraryPath).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.ResourceLibraryResource)
                    .WithMany(p => p.CmsResourceLibraries)
                    .HasForeignKey(d => d.ResourceLibraryResourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ResourceLibrary_CMS_Resource");
            });

            modelBuilder.Entity<CmsResourceTranslation>(entity =>
            {
                entity.HasOne(d => d.TranslationCulture)
                    .WithMany(p => p.CmsResourceTranslations)
                    .HasForeignKey(d => d.TranslationCultureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ResourceTranslation_TranslationCultureID_CMS_Culture");

                entity.HasOne(d => d.TranslationString)
                    .WithMany(p => p.CmsResourceTranslations)
                    .HasForeignKey(d => d.TranslationStringId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_ResourceTranslation_TranslationStringID_CMS_ResourceString");
            });

            modelBuilder.Entity<CmsRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.SiteId, e.RoleName, e.RoleDisplayName }, "IX_CMS_Role_SiteID_RoleName_RoleDisplayName")
                    .IsUnique()
                    .IsClustered();

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CmsRoles)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_CMS_Role_SiteID_CMS_SiteID");

                entity.HasMany(d => d.Permissions)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsRolePermission",
                        l => l.HasOne<CmsPermission>().WithMany().HasForeignKey("PermissionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_RolePermission_PermissionID_CMS_Permission"),
                        r => r.HasOne<CmsRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_RolePermission_RoleID_CMS_Role"),
                        j =>
                        {
                            j.HasKey("RoleId", "PermissionId");

                            j.ToTable("CMS_RolePermission");

                            j.HasIndex(new[] { "PermissionId" }, "IX_CMS_RolePermission_PermissionID");

                            j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");

                            j.IndexerProperty<int>("PermissionId").HasColumnName("PermissionID");
                        });
            });

            modelBuilder.Entity<CmsScheduledTask>(entity =>
            {
                entity.Property(e => e.TaskExecutingServerName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.TaskResource)
                    .WithMany(p => p.CmsScheduledTasks)
                    .HasForeignKey(d => d.TaskResourceId)
                    .HasConstraintName("FK_CMS_ScheduledTask_TaskResourceID_CMS_Resource");

                entity.HasOne(d => d.TaskSite)
                    .WithMany(p => p.CmsScheduledTasks)
                    .HasForeignKey(d => d.TaskSiteId)
                    .HasConstraintName("FK_CMS_ScheduledTask_TaskSiteID_CMS_Site");

                entity.HasOne(d => d.TaskUser)
                    .WithMany(p => p.CmsScheduledTasks)
                    .HasForeignKey(d => d.TaskUserId)
                    .HasConstraintName("FK_CMS_ScheduledTask_TaskUserID_CMS_User");
            });

            modelBuilder.Entity<CmsSearchIndex>(entity =>
            {
                entity.HasKey(e => e.IndexId)
                    .IsClustered(false);

                entity.HasIndex(e => e.IndexDisplayName, "IX_CMS_SearchIndex_IndexDisplayName")
                    .IsClustered();

                entity.Property(e => e.IndexProvider).HasDefaultValueSql("(N'')");

                entity.Property(e => e.IndexType).HasDefaultValueSql("('')");

                entity.HasMany(d => d.IndexCultures)
                    .WithMany(p => p.Indices)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsSearchIndexCulture",
                        l => l.HasOne<CmsCulture>().WithMany().HasForeignKey("IndexCultureId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SearchIndexCulture_IndexCultureID_CMS_Culture"),
                        r => r.HasOne<CmsSearchIndex>().WithMany().HasForeignKey("IndexId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SearchIndexCulture_IndexID_CMS_SearchIndex"),
                        j =>
                        {
                            j.HasKey("IndexId", "IndexCultureId");

                            j.ToTable("CMS_SearchIndexCulture");

                            j.HasIndex(new[] { "IndexCultureId" }, "IX_CMS_SearchIndexCulture_IndexCultureID");

                            j.IndexerProperty<int>("IndexId").HasColumnName("IndexID");

                            j.IndexerProperty<int>("IndexCultureId").HasColumnName("IndexCultureID");
                        });

                entity.HasMany(d => d.IndexSites)
                    .WithMany(p => p.Indices)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsSearchIndexSite",
                        l => l.HasOne<CmsSite>().WithMany().HasForeignKey("IndexSiteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SearchIndexSite_IndexSiteID_CMS_Site"),
                        r => r.HasOne<CmsSearchIndex>().WithMany().HasForeignKey("IndexId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SearchIndexSite_IndexID_CMS_SearchIndex"),
                        j =>
                        {
                            j.HasKey("IndexId", "IndexSiteId");

                            j.ToTable("CMS_SearchIndexSite");

                            j.HasIndex(new[] { "IndexSiteId" }, "IX_CMS_SearchIndexSite_IndexSiteID");

                            j.IndexerProperty<int>("IndexId").HasColumnName("IndexID");

                            j.IndexerProperty<int>("IndexSiteId").HasColumnName("IndexSiteID");
                        });
            });

            modelBuilder.Entity<CmsSearchTask>(entity =>
            {
                entity.HasKey(e => e.SearchTaskId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.SearchTaskPriority, e.SearchTaskStatus, e.SearchTaskServerName }, "IX_CMS_SearchTask_SearchTaskPriority_SearchTaskStatus_SearchTaskServerName")
                    .IsClustered();

                entity.Property(e => e.SearchTaskCreated).HasDefaultValueSql("('4/15/2009 11:23:52 AM')");

                entity.Property(e => e.SearchTaskStatus).HasDefaultValueSql("('')");

                entity.Property(e => e.SearchTaskType).HasDefaultValueSql("('')");

                entity.Property(e => e.SearchTaskValue).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CmsSearchTaskAzure>(entity =>
            {
                entity.Property(e => e.SearchTaskAzureAdditionalData).HasDefaultValueSql("(N'')");

                entity.Property(e => e.SearchTaskAzureCreated).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.SearchTaskAzureType).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsSettingsCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .IsClustered(false);

                entity.HasIndex(e => e.CategoryOrder, "IX_CMS_SettingsCategory_CategoryOrder")
                    .IsClustered();

                entity.Property(e => e.CategoryDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.CategoryIsCustom).HasDefaultValueSql("((0))");

                entity.Property(e => e.CategoryIsGroup).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.CategoryParent)
                    .WithMany(p => p.InverseCategoryParent)
                    .HasForeignKey(d => d.CategoryParentId)
                    .HasConstraintName("FK_CMS_SettingsCategory_CMS_SettingsCategory1");

                entity.HasOne(d => d.CategoryResource)
                    .WithMany(p => p.CmsSettingsCategories)
                    .HasForeignKey(d => d.CategoryResourceId)
                    .HasConstraintName("FK_CMS_SettingsCategory_CategoryResourceID_CMS_Resource");
            });

            modelBuilder.Entity<CmsSettingsKey>(entity =>
            {
                entity.Property(e => e.KeyDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.KeyExplanationText).HasDefaultValueSql("(N'')");

                entity.Property(e => e.KeyIsCustom).HasDefaultValueSql("((0))");

                entity.Property(e => e.KeyIsGlobal).HasDefaultValueSql("((0))");

                entity.Property(e => e.KeyIsHidden).HasDefaultValueSql("((0))");

                entity.Property(e => e.KeyName).HasDefaultValueSql("('')");

                entity.Property(e => e.KeyType).HasDefaultValueSql("('')");

                entity.HasOne(d => d.KeyCategory)
                    .WithMany(p => p.CmsSettingsKeys)
                    .HasForeignKey(d => d.KeyCategoryId)
                    .HasConstraintName("FK_CMS_SettingsKey_KeyCategoryID_CMS_SettingsCategory");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CmsSettingsKeys)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_CMS_SettingsKey_SiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsSite>(entity =>
            {
                entity.HasKey(e => e.SiteId)
                    .IsClustered(false);

                entity.HasIndex(e => e.SiteDisplayName, "IX_CMS_Site_SiteDisplayName")
                    .IsClustered();

                entity.Property(e => e.SiteDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.SiteDomainName).HasDefaultValueSql("('')");

                entity.Property(e => e.SiteLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.SiteName).HasDefaultValueSql("('')");

                entity.Property(e => e.SiteStatus).HasDefaultValueSql("('')");

                entity.HasMany(d => d.Cultures)
                    .WithMany(p => p.Sites)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsSiteCulture",
                        l => l.HasOne<CmsCulture>().WithMany().HasForeignKey("CultureId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SiteCulture_CultureID_CMS_Culture"),
                        r => r.HasOne<CmsSite>().WithMany().HasForeignKey("SiteId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_SiteCulture_SiteID_CMS_Site"),
                        j =>
                        {
                            j.HasKey("SiteId", "CultureId");

                            j.ToTable("CMS_SiteCulture");

                            j.HasIndex(new[] { "CultureId" }, "IX_CMS_SiteCulture_CultureID");

                            j.IndexerProperty<int>("SiteId").HasColumnName("SiteID");

                            j.IndexerProperty<int>("CultureId").HasColumnName("CultureID");
                        });
            });

            modelBuilder.Entity<CmsSiteDomainAlias>(entity =>
            {
                entity.Property(e => e.SiteDomainLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CmsSiteDomainAliases)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_SiteDomainAlias_SiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsState>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .IsClustered(false);

                entity.HasIndex(e => e.StateDisplayName, "IX_CMS_State_CountryID_StateDisplayName")
                    .IsClustered();

                entity.Property(e => e.StateDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.StateName).HasDefaultValueSql("('')");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.CmsStates)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_State_CountryID_CMS_Country");
            });

            modelBuilder.Entity<CmsTimeZone>(entity =>
            {
                entity.HasKey(e => e.TimeZoneId)
                    .IsClustered(false);

                entity.HasIndex(e => e.TimeZoneDisplayName, "IX_CMS_TimeZone_TimeZoneDisplayName")
                    .IsClustered();

                entity.Property(e => e.TimeZoneDaylight).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<CmsTransformation>(entity =>
            {
                entity.HasKey(e => e.TransformationId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.TransformationClassId, e.TransformationName }, "IX_CMS_Transformation_TransformationClassID_TransformationName")
                    .IsClustered();

                entity.Property(e => e.TransformationCode).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TransformationName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TransformationType).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.TransformationClass)
                    .WithMany(p => p.CmsTransformations)
                    .HasForeignKey(d => d.TransformationClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Transformation_TransformationClassID_CMS_Class");
            });

            modelBuilder.Entity<CmsTree>(entity =>
            {
                entity.Property(e => e.NodeHasChildren).HasDefaultValueSql("((0))");

                entity.Property(e => e.NodeHasLinks).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.NodeClass)
                    .WithMany(p => p.CmsTrees)
                    .HasForeignKey(d => d.NodeClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Tree_NodeClassID_CMS_Class");

                entity.HasOne(d => d.NodeLinkedNode)
                    .WithMany(p => p.InverseNodeLinkedNode)
                    .HasForeignKey(d => d.NodeLinkedNodeId)
                    .HasConstraintName("FK_CMS_Tree_NodeLinkedNodeID_CMS_Tree");

                entity.HasOne(d => d.NodeLinkedNodeSite)
                    .WithMany(p => p.CmsTreeNodeLinkedNodeSites)
                    .HasForeignKey(d => d.NodeLinkedNodeSiteId)
                    .HasConstraintName("FK_CMS_Tree_NodeLinkedNodeSiteID_CMS_Site");

                entity.HasOne(d => d.NodeOriginalNode)
                    .WithMany(p => p.InverseNodeOriginalNode)
                    .HasForeignKey(d => d.NodeOriginalNodeId)
                    .HasConstraintName("FK_CMS_Tree_NodeOriginalNodeID_CMS_Tree");

                entity.HasOne(d => d.NodeOwnerNavigation)
                    .WithMany(p => p.CmsTrees)
                    .HasForeignKey(d => d.NodeOwner)
                    .HasConstraintName("FK_CMS_Tree_NodeOwner_CMS_User");

                entity.HasOne(d => d.NodeParent)
                    .WithMany(p => p.InverseNodeParent)
                    .HasForeignKey(d => d.NodeParentId)
                    .HasConstraintName("FK_CMS_Tree_NodeParentID_CMS_Tree");

                entity.HasOne(d => d.NodeSite)
                    .WithMany(p => p.CmsTreeNodeSites)
                    .HasForeignKey(d => d.NodeSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_Tree_NodeSiteID_CMS_Site");
            });

            modelBuilder.Entity<CmsUser>(entity =>
            {
                entity.Property(e => e.UserName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.UserPassword).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsUserCulture>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CultureId, e.SiteId });

                entity.HasOne(d => d.Culture)
                    .WithMany(p => p.CmsUserCultures)
                    .HasForeignKey(d => d.CultureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserCulture_CultureID_CMS_Culture");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CmsUserCultures)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserCulture_SiteID_CMS_Site");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsUserCultures)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserCulture_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsUserMacroIdentity>(entity =>
            {
                entity.Property(e => e.UserMacroIdentityLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.HasOne(d => d.UserMacroIdentityMacroIdentity)
                    .WithMany(p => p.CmsUserMacroIdentities)
                    .HasForeignKey(d => d.UserMacroIdentityMacroIdentityId)
                    .HasConstraintName("FK_CMS_UserMacroIdentity_UserMacroIdentityMacroIdentityID_CMS_MacroIdentity");

                entity.HasOne(d => d.UserMacroIdentityUser)
                    .WithOne(p => p.CmsUserMacroIdentity)
                    .HasForeignKey<CmsUserMacroIdentity>(d => d.UserMacroIdentityUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserMacroIdentity_UserMacroIdentityUserID_CMS_User");
            });

            modelBuilder.Entity<CmsUserRole>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.CmsUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserRole_RoleID_CMS_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserRole_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsUserSite>(entity =>
            {
                entity.HasOne(d => d.Site)
                    .WithMany(p => p.CmsUserSites)
                    .HasForeignKey(d => d.SiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserSite_SiteID_CMS_Site");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsUserSites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_UserSite_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsVersionHistory>(entity =>
            {
                entity.HasKey(e => e.VersionHistoryId)
                    .IsClustered(false);

                entity.HasIndex(e => e.DocumentId, "IX_CMS_VersionHistory_DocumentID")
                    .IsClustered();

                entity.HasOne(d => d.ModifiedByUser)
                    .WithMany(p => p.CmsVersionHistoryModifiedByUsers)
                    .HasForeignKey(d => d.ModifiedByUserId)
                    .HasConstraintName("FK_CMS_VersionHistory_ModifiedByUserID_CMS_User");

                entity.HasOne(d => d.NodeSite)
                    .WithMany(p => p.CmsVersionHistories)
                    .HasForeignKey(d => d.NodeSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_VersionHistory_NodeSiteID_CMS_Site");

                entity.HasOne(d => d.VersionClass)
                    .WithMany(p => p.CmsVersionHistories)
                    .HasForeignKey(d => d.VersionClassId)
                    .HasConstraintName("FK_CMS_VersionHistory_VersionClassID_CMS_Class");

                entity.HasOne(d => d.VersionDeletedByUser)
                    .WithMany(p => p.CmsVersionHistoryVersionDeletedByUsers)
                    .HasForeignKey(d => d.VersionDeletedByUserId)
                    .HasConstraintName("FK_CMS_VersionHistory_DeletedByUserID_CMS_User");

                entity.HasOne(d => d.VersionWorkflow)
                    .WithMany(p => p.CmsVersionHistories)
                    .HasForeignKey(d => d.VersionWorkflowId)
                    .HasConstraintName("FK_CMS_VersionHistory_VersionWorkflowID_CMS_Workflow");

                entity.HasOne(d => d.VersionWorkflowStep)
                    .WithMany(p => p.CmsVersionHistories)
                    .HasForeignKey(d => d.VersionWorkflowStepId)
                    .HasConstraintName("FK_CMS_VersionHistory_VersionWorkflowStepID_CMS_WorkflowStep");
            });

            modelBuilder.Entity<CmsWebFarmServer>(entity =>
            {
                entity.Property(e => e.ServerDisplayName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ServerLastModified).HasDefaultValueSql("('9/17/2013 12:18:06 PM')");

                entity.Property(e => e.ServerName).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsWebFarmServerLog>(entity =>
            {
                entity.Property(e => e.LogCode).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsWebFarmServerTask>(entity =>
            {
                entity.HasKey(e => new { e.ServerId, e.TaskId });

                entity.HasOne(d => d.Server)
                    .WithMany(p => p.CmsWebFarmServerTasks)
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WebFarmServerTask_ServerID_CMS_WebFarmServer");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.CmsWebFarmServerTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WebFarmServerTask_TaskID_CMS_WebFarmTask");
            });

            modelBuilder.Entity<CmsWebFarmTask>(entity =>
            {
                entity.Property(e => e.TaskGuid).HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");

                entity.Property(e => e.TaskIsMemory).HasDefaultValueSql("((0))");

                entity.Property(e => e.TaskType).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<CmsWorkflow>(entity =>
            {
                entity.HasKey(e => e.WorkflowId)
                    .IsClustered(false);

                entity.HasIndex(e => e.WorkflowDisplayName, "IX_CMS_Workflow_WorkflowDisplayName")
                    .IsClustered();

                entity.Property(e => e.WorkflowAutoPublishChanges).HasDefaultValueSql("((0))");

                entity.Property(e => e.WorkflowDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.WorkflowEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowName).HasDefaultValueSql("('')");

                entity.Property(e => e.WorkflowSendApproveEmails).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowSendArchiveEmails).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowSendPublishEmails).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowSendReadyForApprovalEmails).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowSendRejectEmails).HasDefaultValueSql("((1))");

                entity.Property(e => e.WorkflowUseCheckinCheckout).HasDefaultValueSql("((0))");

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Workflows)
                    .UsingEntity<Dictionary<string, object>>(
                        "CmsWorkflowUser",
                        l => l.HasOne<CmsUser>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_WorkflowUser_UserID_CMS_User"),
                        r => r.HasOne<CmsWorkflow>().WithMany().HasForeignKey("WorkflowId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_CMS_WorkflowUser_WorkflowID_CMS_Workflow"),
                        j =>
                        {
                            j.HasKey("WorkflowId", "UserId").HasName("PK_CMS_WorkflowUser_1");

                            j.ToTable("CMS_WorkflowUser");

                            j.HasIndex(new[] { "UserId" }, "IX_CMS_WorkflowUser_UserID");

                            j.IndexerProperty<int>("WorkflowId").HasColumnName("WorkflowID");

                            j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                        });
            });

            modelBuilder.Entity<CmsWorkflowAction>(entity =>
            {
                entity.Property(e => e.ActionEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.ActionWorkflowType).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ActionResource)
                    .WithMany(p => p.CmsWorkflowActions)
                    .HasForeignKey(d => d.ActionResourceId)
                    .HasConstraintName("FK_CMS_WorkflowAction_ActionResourceID");
            });

            modelBuilder.Entity<CmsWorkflowHistory>(entity =>
            {
                entity.Property(e => e.HistoryRejected).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ApprovedByUser)
                    .WithMany(p => p.CmsWorkflowHistories)
                    .HasForeignKey(d => d.ApprovedByUserId)
                    .HasConstraintName("FK_CMS_WorkflowHistory_ApprovedByUserID_CMS_User");

                entity.HasOne(d => d.HistoryWorkflow)
                    .WithMany(p => p.CmsWorkflowHistories)
                    .HasForeignKey(d => d.HistoryWorkflowId)
                    .HasConstraintName("FK_CMS_WorkflowHistory_HistoryWorkflowID_CMS_Workflow");

                entity.HasOne(d => d.Step)
                    .WithMany(p => p.CmsWorkflowHistorySteps)
                    .HasForeignKey(d => d.StepId)
                    .HasConstraintName("FK_CMS_WorkflowHistory_StepID_CMS_WorkflowStep");

                entity.HasOne(d => d.TargetStep)
                    .WithMany(p => p.CmsWorkflowHistoryTargetSteps)
                    .HasForeignKey(d => d.TargetStepId)
                    .HasConstraintName("FK_CMS_WorkflowHistory_TargetStepID_CMS_WorkflowStep");

                entity.HasOne(d => d.VersionHistory)
                    .WithMany(p => p.CmsWorkflowHistories)
                    .HasForeignKey(d => d.VersionHistoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowHistory_VersionHistoryID_CMS_VersionHistory");
            });

            modelBuilder.Entity<CmsWorkflowScope>(entity =>
            {
                entity.HasKey(e => e.ScopeId)
                    .IsClustered(false);

                entity.HasIndex(e => e.ScopeStartingPath, "IX_CMS_WorkflowScope_ScopeStartingPath")
                    .IsClustered();

                entity.HasOne(d => d.ScopeClass)
                    .WithMany(p => p.CmsWorkflowScopes)
                    .HasForeignKey(d => d.ScopeClassId)
                    .HasConstraintName("FK_CMS_WorkflowScope_ScopeClassID_CMS_Class");

                entity.HasOne(d => d.ScopeCulture)
                    .WithMany(p => p.CmsWorkflowScopes)
                    .HasForeignKey(d => d.ScopeCultureId)
                    .HasConstraintName("FK_CMS_WorkflowScope_ScopeCultureID_CMS_Culture");

                entity.HasOne(d => d.ScopeSite)
                    .WithMany(p => p.CmsWorkflowScopes)
                    .HasForeignKey(d => d.ScopeSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowScope_ScopeSiteID_CMS_Site");

                entity.HasOne(d => d.ScopeWorkflow)
                    .WithMany(p => p.CmsWorkflowScopes)
                    .HasForeignKey(d => d.ScopeWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowScope_ScopeWorkflowID_CMS_WorkflowID");
            });

            modelBuilder.Entity<CmsWorkflowStep>(entity =>
            {
                entity.Property(e => e.StepAllowPublish).HasDefaultValueSql("((0))");

                entity.Property(e => e.StepAllowReject).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.StepAction)
                    .WithMany(p => p.CmsWorkflowSteps)
                    .HasForeignKey(d => d.StepActionId)
                    .HasConstraintName("FK_CMS_WorkflowStep_StepActionID");

                entity.HasOne(d => d.StepWorkflow)
                    .WithMany(p => p.CmsWorkflowSteps)
                    .HasForeignKey(d => d.StepWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowStep_StepWorkflowID");
            });

            modelBuilder.Entity<CmsWorkflowStepRole>(entity =>
            {
                entity.HasKey(e => e.WorkflowStepRoleId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.StepId, e.StepSourcePointGuid, e.RoleId }, "IX_CMS_WorkflowStepRoles_StepID_StepSourcePointGUID_RoleID")
                    .IsUnique()
                    .IsClustered();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.CmsWorkflowStepRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowStepRoles_RoleID_CMS_Role");

                entity.HasOne(d => d.Step)
                    .WithMany(p => p.CmsWorkflowStepRoles)
                    .HasForeignKey(d => d.StepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowStepRoles_StepID_CMS_WorkflowStep");
            });

            modelBuilder.Entity<CmsWorkflowStepUser>(entity =>
            {
                entity.HasKey(e => e.WorkflowStepUserId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.StepId, e.StepSourcePointGuid, e.UserId }, "IX_CMS_WorkflowStepUser_StepID_StepSourcePointGUID_UserID")
                    .IsUnique()
                    .IsClustered();

                entity.HasOne(d => d.Step)
                    .WithMany(p => p.CmsWorkflowStepUsers)
                    .HasForeignKey(d => d.StepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowStepUser_StepID_CMS_WorkflowStep");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CmsWorkflowStepUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowStepUser_UserID_CMS_User");
            });

            modelBuilder.Entity<CmsWorkflowTransition>(entity =>
            {
                entity.HasOne(d => d.TransitionEndStep)
                    .WithMany(p => p.CmsWorkflowTransitionTransitionEndSteps)
                    .HasForeignKey(d => d.TransitionEndStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowTransition_TransitionEndStepID_CMS_WorkflowStep");

                entity.HasOne(d => d.TransitionStartStep)
                    .WithMany(p => p.CmsWorkflowTransitionTransitionStartSteps)
                    .HasForeignKey(d => d.TransitionStartStepId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowTransition_TransitionStartStepID_CMS_WorkflowStep");

                entity.HasOne(d => d.TransitionWorkflow)
                    .WithMany(p => p.CmsWorkflowTransitions)
                    .HasForeignKey(d => d.TransitionWorkflowId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CMS_WorkflowTransition_TransitionWorkflowID_CMS_Workflow");
            });

            modelBuilder.Entity<DancingGoatCoreAboutUsSection>(entity =>
            {
                entity.Property(e => e.AboutUsSectionHeading).HasDefaultValueSql("(N'')");

                entity.Property(e => e.AboutUsSectionText).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreArticle>(entity =>
            {
                entity.Property(e => e.ArticleSummary).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ArticleText).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ArticleTitle).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreCafe>(entity =>
            {
                entity.Property(e => e.CafeCity).HasDefaultValueSql("(N'')");

                entity.Property(e => e.CafeCountry).HasDefaultValueSql("(N'USA')");

                entity.Property(e => e.CafeIsCompanyCafe).HasDefaultValueSql("((0))");

                entity.Property(e => e.CafeName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.CafePhone).HasDefaultValueSql("(N'')");

                entity.Property(e => e.CafeStreet).HasDefaultValueSql("(N'')");

                entity.Property(e => e.CafeZipCode).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreCoffee>(entity =>
            {
                entity.Property(e => e.CoffeeDescription).HasDefaultValueSql("(N'')");

                entity.Property(e => e.CoffeeShortDescription).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreContact>(entity =>
            {
                entity.Property(e => e.ContactCity).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ContactCountry).HasDefaultValueSql("(N'USA')");

                entity.Property(e => e.ContactEmail).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ContactName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ContactPhone).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ContactStreet).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ContactZipCode).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreHomeSection>(entity =>
            {
                entity.Property(e => e.HomeSectionHeading).HasDefaultValueSql("(N'')");

                entity.Property(e => e.HomeSectionText).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreNavigationItem>(entity =>
            {
                entity.Property(e => e.LinkTo).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreReference>(entity =>
            {
                entity.Property(e => e.ReferenceDescription).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ReferenceImage).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ReferenceName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.ReferenceText).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<DancingGoatCoreSocialLink>(entity =>
            {
                entity.Property(e => e.SocialLinkIcon).HasDefaultValueSql("(N'')");

                entity.Property(e => e.SocialLinkName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.SocialLinkTitle).HasDefaultValueSql("(N'Follow us')");

                entity.Property(e => e.SocialLinkUrl).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<ExportHistory>(entity =>
            {
                entity.HasKey(e => e.ExportId)
                    .IsClustered(false);

                entity.HasIndex(e => e.ExportDateTime, "IX_Export_History_ExportDateTime")
                    .IsClustered();

                entity.Property(e => e.ExportFileName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.ExportSite)
                    .WithMany(p => p.ExportHistories)
                    .HasForeignKey(d => d.ExportSiteId)
                    .HasConstraintName("FK_Export_History_ExportSiteID_CMS_Site");

                entity.HasOne(d => d.ExportUser)
                    .WithMany(p => p.ExportHistories)
                    .HasForeignKey(d => d.ExportUserId)
                    .HasConstraintName("FK_Export_History_ExportUserID_CMS_User");
            });

            modelBuilder.Entity<ExportTask>(entity =>
            {
                entity.HasOne(d => d.TaskSite)
                    .WithMany(p => p.ExportTasks)
                    .HasForeignKey(d => d.TaskSiteId)
                    .HasConstraintName("FK_Export_Task_TaskSiteID_CMS_Site");
            });

            modelBuilder.Entity<FormDancingGoatCoreCoffeeSampleList>(entity =>
            {
                entity.Property(e => e.Address).HasDefaultValueSql("(N'')");

                entity.Property(e => e.City).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Country).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Email).HasDefaultValueSql("(N'')");

                entity.Property(e => e.FirstName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.FormInserted).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.FormUpdated).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.LastName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.Zipcode).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<FormDancingGoatCoreContactUsNew>(entity =>
            {
                entity.Property(e => e.FormInserted).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.FormUpdated).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");

                entity.Property(e => e.UserEmail).HasDefaultValueSql("(N'')");

                entity.Property(e => e.UserMessage).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<IntegrationConnector>(entity =>
            {
                entity.HasKey(e => e.ConnectorId)
                    .IsClustered(false);

                entity.HasIndex(e => e.ConnectorDisplayName, "IX_Integration_Connector_ConnectorDisplayName")
                    .IsClustered();

                entity.Property(e => e.ConnectorEnabled).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<IntegrationSyncLog>(entity =>
            {
                entity.HasOne(d => d.SyncLogSynchronization)
                    .WithMany(p => p.IntegrationSyncLogs)
                    .HasForeignKey(d => d.SyncLogSynchronizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Integration_SyncLog_SyncLogSynchronizationID_Integration_Synchronization");
            });

            modelBuilder.Entity<IntegrationSynchronization>(entity =>
            {
                entity.HasOne(d => d.SynchronizationConnector)
                    .WithMany(p => p.IntegrationSynchronizations)
                    .HasForeignKey(d => d.SynchronizationConnectorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Integration_Synchronization_SynchronizationConnectorID_Integration_Connector");

                entity.HasOne(d => d.SynchronizationTask)
                    .WithMany(p => p.IntegrationSynchronizations)
                    .HasForeignKey(d => d.SynchronizationTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Integration_Synchronization_SynchronizationTaskID_Integration_Task");
            });

            modelBuilder.Entity<IntegrationTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .IsClustered(false);

                entity.HasIndex(e => e.TaskNodeAliasPath, "IX_Integration_Task_TaskNodeAliasPath")
                    .IsClustered();

                entity.HasOne(d => d.TaskSite)
                    .WithMany(p => p.IntegrationTasks)
                    .HasForeignKey(d => d.TaskSiteId)
                    .HasConstraintName("FK_IntegrationTask_TaskSiteID_CMS_Site");
            });

            modelBuilder.Entity<MediaFile>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .IsClustered(false);

                entity.HasIndex(e => e.FilePath, "IX_Media_File_FilePath")
                    .IsClustered();

                entity.Property(e => e.FileCreatedWhen).HasDefaultValueSql("('11/11/2008 4:10:00 PM')");

                entity.Property(e => e.FileModifiedWhen).HasDefaultValueSql("('11/11/2008 4:11:15 PM')");

                entity.Property(e => e.FileTitle).HasDefaultValueSql("('')");

                entity.HasOne(d => d.FileCreatedByUser)
                    .WithMany(p => p.MediaFileFileCreatedByUsers)
                    .HasForeignKey(d => d.FileCreatedByUserId)
                    .HasConstraintName("FK_Media_File_FileCreatedByUserID_CMS_User");

                entity.HasOne(d => d.FileLibrary)
                    .WithMany(p => p.MediaFiles)
                    .HasForeignKey(d => d.FileLibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_File_FileLibraryID_Media_Library");

                entity.HasOne(d => d.FileModifiedByUser)
                    .WithMany(p => p.MediaFileFileModifiedByUsers)
                    .HasForeignKey(d => d.FileModifiedByUserId)
                    .HasConstraintName("FK_Media_File_FileModifiedByUserID_CMS_User");

                entity.HasOne(d => d.FileSite)
                    .WithMany(p => p.MediaFiles)
                    .HasForeignKey(d => d.FileSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_File_FileSiteID_CMS_Site");
            });

            modelBuilder.Entity<MediaLibrary>(entity =>
            {
                entity.HasKey(e => e.LibraryId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.LibrarySiteId, e.LibraryDisplayName }, "IX_Media_Library_LibraryDisplayName")
                    .IsClustered();

                entity.Property(e => e.LibraryName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.LibrarySite)
                    .WithMany(p => p.MediaLibraries)
                    .HasForeignKey(d => d.LibrarySiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_Library_LibrarySiteID_CMS_Site");
            });

            modelBuilder.Entity<MediaLibraryRolePermission>(entity =>
            {
                entity.HasKey(e => new { e.LibraryId, e.RoleId, e.PermissionId });

                entity.HasOne(d => d.Library)
                    .WithMany(p => p.MediaLibraryRolePermissions)
                    .HasForeignKey(d => d.LibraryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_LibraryRolePermission_LibraryID_Media_Library");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.MediaLibraryRolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_LibraryRolePermission_PermissionID_CMS_Permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.MediaLibraryRolePermissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Media_LibraryRolePermission_RoleID_CMS_Role");
            });

            modelBuilder.Entity<OmAbtest>(entity =>
            {
                entity.Property(e => e.AbtestDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.AbtestIncludedTraffic).HasDefaultValueSql("((100))");

                entity.Property(e => e.AbtestName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.AbtestOriginalPage).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.AbtestSite)
                    .WithMany(p => p.OmAbtests)
                    .HasForeignKey(d => d.AbtestSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_ABTest_SiteID_CMS_Site");
            });

            modelBuilder.Entity<OmAbvariantDatum>(entity =>
            {
                entity.Property(e => e.AbvariantDisplayName).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.AbvariantTest)
                    .WithMany(p => p.OmAbvariantData)
                    .HasForeignKey(d => d.AbvariantTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_ABVariantData_ABVariantTestID_OM_ABTest");
            });

            modelBuilder.Entity<OmAccount>(entity =>
            {
                entity.HasOne(d => d.AccountCountry)
                    .WithMany(p => p.OmAccounts)
                    .HasForeignKey(d => d.AccountCountryId)
                    .HasConstraintName("FK_OM_Account_CMS_Country");

                entity.HasOne(d => d.AccountOwnerUser)
                    .WithMany(p => p.OmAccounts)
                    .HasForeignKey(d => d.AccountOwnerUserId)
                    .HasConstraintName("FK_OM_Account_CMS_User");

                entity.HasOne(d => d.AccountPrimaryContact)
                    .WithMany(p => p.OmAccountAccountPrimaryContacts)
                    .HasForeignKey(d => d.AccountPrimaryContactId)
                    .HasConstraintName("FK_OM_Account_OM_Contact_PrimaryContact");

                entity.HasOne(d => d.AccountSecondaryContact)
                    .WithMany(p => p.OmAccountAccountSecondaryContacts)
                    .HasForeignKey(d => d.AccountSecondaryContactId)
                    .HasConstraintName("FK_OM_Account_OM_Contact_SecondaryContact");

                entity.HasOne(d => d.AccountState)
                    .WithMany(p => p.OmAccounts)
                    .HasForeignKey(d => d.AccountStateId)
                    .HasConstraintName("FK_OM_Account_CMS_State");

                entity.HasOne(d => d.AccountStatus)
                    .WithMany(p => p.OmAccounts)
                    .HasForeignKey(d => d.AccountStatusId)
                    .HasConstraintName("FK_OM_Account_OM_AccountStatus");

                entity.HasOne(d => d.AccountSubsidiaryOf)
                    .WithMany(p => p.InverseAccountSubsidiaryOf)
                    .HasForeignKey(d => d.AccountSubsidiaryOfId)
                    .HasConstraintName("FK_OM_Account_OM_Account_SubsidiaryOf");
            });

            modelBuilder.Entity<OmAccountContact>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.OmAccountContacts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_AccountContact_OM_Account");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.OmAccountContacts)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_AccountContact_OM_Contact");

                entity.HasOne(d => d.ContactRole)
                    .WithMany(p => p.OmAccountContacts)
                    .HasForeignKey(d => d.ContactRoleId)
                    .HasConstraintName("FK_OM_AccountContact_OM_ContactRole");
            });

            modelBuilder.Entity<OmActivity>(entity =>
            {
                entity.HasIndex(e => e.ActivityCampaign, "IX_OM_Activity_ActivityCampaign")
                    .HasFilter("([ActivityCampaign] IS NOT NULL)")
                    .HasFillFactor(90);
            });

            modelBuilder.Entity<OmActivityType>(entity =>
            {
                entity.Property(e => e.ActivityTypeEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.ActivityTypeIsCustom).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<OmContact>(entity =>
            {
                entity.Property(e => e.ContactCreated).HasDefaultValueSql("('5/3/2011 10:51:13 AM')");

                entity.Property(e => e.ContactMonitored).HasDefaultValueSql("((0))");

                entity.Property(e => e.ContactSalesForceLeadReplicationDisabled).HasDefaultValueSql("((0))");

                entity.Property(e => e.ContactSalesForceLeadReplicationRequired).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ContactCountry)
                    .WithMany(p => p.OmContacts)
                    .HasForeignKey(d => d.ContactCountryId)
                    .HasConstraintName("FK_OM_Contact_CMS_Country");

                entity.HasOne(d => d.ContactOwnerUser)
                    .WithMany(p => p.OmContacts)
                    .HasForeignKey(d => d.ContactOwnerUserId)
                    .HasConstraintName("FK_OM_Contact_CMS_User");

                entity.HasOne(d => d.ContactState)
                    .WithMany(p => p.OmContacts)
                    .HasForeignKey(d => d.ContactStateId)
                    .HasConstraintName("FK_OM_Contact_CMS_State");

                entity.HasOne(d => d.ContactStatus)
                    .WithMany(p => p.OmContacts)
                    .HasForeignKey(d => d.ContactStatusId)
                    .HasConstraintName("FK_OM_Contact_OM_ContactStatus");
            });

            modelBuilder.Entity<OmContactGroup>(entity =>
            {
                entity.HasKey(e => e.ContactGroupId)
                    .HasName("PK_CMS_ContactGroup");

                entity.Property(e => e.ContactGroupName).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<OmContactGroupMember>(entity =>
            {
                entity.Property(e => e.ContactGroupMemberFromCondition).HasDefaultValueSql("((0))");

                entity.Property(e => e.ContactGroupMemberFromManual).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ContactGroupMemberContactGroup)
                    .WithMany(p => p.OmContactGroupMembers)
                    .HasForeignKey(d => d.ContactGroupMemberContactGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_ContactGroupMembers_OM_ContactGroup");
            });

            modelBuilder.Entity<OmContactRole>(entity =>
            {
                entity.Property(e => e.ContactRoleDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.ContactRoleName).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<OmContactStatus>(entity =>
            {
                entity.Property(e => e.ContactStatusDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.ContactStatusName).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<OmMembership>(entity =>
            {
                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.OmMemberships)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_Membership_OM_Contact");
            });

            modelBuilder.Entity<OmTrackedWebsite>(entity =>
            {
                entity.Property(e => e.TrackedWebsiteDisplayName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TrackedWebsiteName).HasDefaultValueSql("(N'')");

                entity.Property(e => e.TrackedWebsiteUrl).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.TrackedWebsiteSite)
                    .WithMany(p => p.OmTrackedWebsites)
                    .HasForeignKey(d => d.TrackedWebsiteSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_TrackedWebsite_TrackedWebsiteSiteID_CMS_Site");
            });

            modelBuilder.Entity<OmVisitorToContact>(entity =>
            {
                entity.HasOne(d => d.VisitorToContactContact)
                    .WithMany(p => p.OmVisitorToContacts)
                    .HasForeignKey(d => d.VisitorToContactContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OM_VisitorToContact_OM_Contact_Cascade");
            });

            modelBuilder.Entity<StagingServer>(entity =>
            {
                entity.HasKey(e => e.ServerId)
                    .IsClustered(false);

                entity.HasIndex(e => new { e.ServerSiteId, e.ServerDisplayName }, "IX_Staging_Server_ServerSiteID_ServerDisplayName")
                    .IsClustered();

                entity.Property(e => e.ServerAuthentication).HasDefaultValueSql("('USERNAME')");

                entity.Property(e => e.ServerDisplayName).HasDefaultValueSql("('')");

                entity.Property(e => e.ServerEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.ServerName).HasDefaultValueSql("('')");

                entity.Property(e => e.ServerUrl).HasDefaultValueSql("(N'')");

                entity.HasOne(d => d.ServerSite)
                    .WithMany(p => p.StagingServers)
                    .HasForeignKey(d => d.ServerSiteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_Server_ServerSiteID_CMS_Site");
            });

            modelBuilder.Entity<StagingSynchronization>(entity =>
            {
                entity.HasOne(d => d.SynchronizationServer)
                    .WithMany(p => p.StagingSynchronizations)
                    .HasForeignKey(d => d.SynchronizationServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_Synchronization_SynchronizationServerID_Staging_Server");

                entity.HasOne(d => d.SynchronizationTask)
                    .WithMany(p => p.StagingSynchronizations)
                    .HasForeignKey(d => d.SynchronizationTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_Synchronization_SynchronizationTaskID_Staging_Task");
            });

            modelBuilder.Entity<StagingTask>(entity =>
            {
                entity.Property(e => e.TaskServers).HasDefaultValueSql("('null')");

                entity.HasOne(d => d.TaskSite)
                    .WithMany(p => p.StagingTasks)
                    .HasForeignKey(d => d.TaskSiteId)
                    .HasConstraintName("FK_Staging_Task_TaskSiteID_CMS_Site");
            });

            modelBuilder.Entity<StagingTaskGroup>(entity =>
            {
                entity.Property(e => e.TaskGroupCodeName).HasDefaultValueSql("(N'')");
            });

            modelBuilder.Entity<StagingTaskGroupTask>(entity =>
            {
                entity.HasOne(d => d.TaskGroup)
                    .WithMany(p => p.StagingTaskGroupTasks)
                    .HasForeignKey(d => d.TaskGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskGroupTask_Staging_TaskGroup");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.StagingTaskGroupTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskGroupTask_Staging_Task");
            });

            modelBuilder.Entity<StagingTaskGroupUser>(entity =>
            {
                entity.HasOne(d => d.TaskGroup)
                    .WithMany(p => p.StagingTaskGroupUsers)
                    .HasForeignKey(d => d.TaskGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskGroupUser_Staging_TaskGroup");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.StagingTaskGroupUser)
                    .HasForeignKey<StagingTaskGroupUser>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskGroupUser_CMS_User");
            });

            modelBuilder.Entity<StagingTaskUser>(entity =>
            {
                entity.HasOne(d => d.Task)
                    .WithMany(p => p.StagingTaskUsers)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskUser_StagingTask");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StagingTaskUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staging_TaskUser_CMS_User");
            });

            modelBuilder.Entity<TempFile>(entity =>
            {
                entity.Property(e => e.FileDirectory).HasDefaultValueSql("('')");

                entity.Property(e => e.FileExtension).HasDefaultValueSql("('')");

                entity.Property(e => e.FileLastModified).HasDefaultValueSql("('6/29/2010 1:57:54 PM')");

                entity.Property(e => e.FileMimeType).HasDefaultValueSql("('')");

                entity.Property(e => e.FileName).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TempPageBuilderWidget>(entity =>
            {
                entity.Property(e => e.PageBuilderWidgetsLastModified).HasDefaultValueSql("('1/1/0001 12:00:00 AM')");
            });

            modelBuilder.Entity<ViewCmsObjectVersionHistoryUserJoined>(entity =>
            {
                entity.ToView("View_CMS_ObjectVersionHistoryUser_Joined");
            });

            modelBuilder.Entity<ViewCmsResourceStringJoined>(entity =>
            {
                entity.ToView("View_CMS_ResourceString_Joined");
            });

            modelBuilder.Entity<ViewCmsResourceTranslatedJoined>(entity =>
            {
                entity.ToView("View_CMS_ResourceTranslated_Joined");
            });

            modelBuilder.Entity<ViewCmsRoleResourcePermissionJoined>(entity =>
            {
                entity.ToView("View_CMS_RoleResourcePermission_Joined");
            });

            modelBuilder.Entity<ViewCmsSiteDocumentCount>(entity =>
            {
                entity.ToView("View_CMS_Site_DocumentCount");

                entity.Property(e => e.SiteId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<ViewCmsTreeJoined>(entity =>
            {
                entity.ToView("View_CMS_Tree_Joined");
            });

            modelBuilder.Entity<ViewCmsUserDocument>(entity =>
            {
                entity.ToView("View_CMS_UserDocuments");
            });

            modelBuilder.Entity<ViewCmsUserRoleJoined>(entity =>
            {
                entity.ToView("View_CMS_UserRole_Joined");
            });

            modelBuilder.Entity<ViewCmsUserRoleMembershipRole>(entity =>
            {
                entity.ToView("View_CMS_UserRoleMembershipRole");
            });

            modelBuilder.Entity<ViewCmsUserRoleMembershipRoleValidOnlyJoined>(entity =>
            {
                entity.ToView("View_CMS_UserRole_MembershipRole_ValidOnly_Joined");
            });

            modelBuilder.Entity<ViewIntegrationTaskJoined>(entity =>
            {
                entity.ToView("View_Integration_Task_Joined");
            });

            modelBuilder.Entity<ViewMembershipMembershipUserJoined>(entity =>
            {
                entity.ToView("View_Membership_MembershipUser_Joined");
            });

            modelBuilder.Entity<ViewOmAccountContactAccountJoined>(entity =>
            {
                entity.ToView("View_OM_AccountContact_AccountJoined");
            });

            modelBuilder.Entity<ViewOmAccountContactContactJoined>(entity =>
            {
                entity.ToView("View_OM_AccountContact_ContactJoined");
            });

            modelBuilder.Entity<ViewOmAccountJoined>(entity =>
            {
                entity.ToView("View_OM_Account_Joined");
            });

            modelBuilder.Entity<ViewOmContactGroupMemberAccountJoined>(entity =>
            {
                entity.ToView("View_OM_ContactGroupMember_AccountJoined");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
