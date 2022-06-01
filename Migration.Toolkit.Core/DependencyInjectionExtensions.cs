using CMS.DataEngine;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Behaviors;
using Migration.Toolkit.Core.CmsResource;
using Migration.Toolkit.Core.CmsSettingsKey;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrateDataProtection;
using Migration.Toolkit.Core.MigrateForms;
using Migration.Toolkit.Core.MigrateMediaLibraries;
using Migration.Toolkit.Core.MigratePages;
using Migration.Toolkit.Core.MigratePageTypes;
using Migration.Toolkit.Core.MigrateSettingKeys;
using Migration.Toolkit.Core.MigrateSites;
using Migration.Toolkit.Core.MigrateUsers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services;
using Migration.Toolkit.Core.Services.BulkCopy;

namespace Migration.Toolkit.Core;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseToolkitCore(this IServiceCollection services)
    {
        services.AddSingleton<IMigrationProtocol, DebugMigrationProtocol>();
        // services.AddSingleton<IMigrationProtocol, NullMigrationProtocol>();
        services.AddScoped<IPrimaryKeyLocatorService, PrimaryKeyLocatorService>();
        services.AddTransient<BulkDataCopyService>();

        services.AddSingleton(s => new TableReflectionService(s.GetRequiredService<ILogger<TableReflectionService>>()));
        
        services.AddScoped<PrimaryKeyMappingContext>();

        services.AddTransient<IDataEqualityComparer<Migration.Toolkit.KX13.Models.CmsSettingsKey, Migration.Toolkit.KXO.Models.CmsSettingsKey>, CmsSettingsKeyComparer>();
        services.AddTransient<IEntityMapper<Migration.Toolkit.KX13.Models.CmsSettingsKey, Migration.Toolkit.KXO.Models.CmsSettingsKey>, CmsSettingsKeyMapper>();


        // page type synchronizer
        services.AddTransient<MigratePageTypesCommandHandler>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsClass, KXO.Models.CmsClass>, CmsClassMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsClass, DataClassInfo>, CmsClassMapper>();
        
        // setting keys migrate command
        services.AddTransient<MigrateSettingKeysCommandHandler>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsSettingsCategory, KXO.Models.CmsSettingsCategory>, CmsSettingsCategoryMapper>();
        
        // cms resource
        services.AddTransient<IEntityMapper<Migration.Toolkit.KX13.Models.CmsResource, Migration.Toolkit.KXO.Models.CmsResource>, CmsResourceMapper>();
        
        // cms user
        services.AddTransient<IEntityMapper<KX13.Models.CmsUser, KXO.Models.CmsUser>, CmsUserMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsRole, KXO.Models.CmsRole>, CmsRoleMapper>();
        services.AddTransient<MigrateUsersCommandHandler>();

        // cms web farm
        // services.AddTransient<IEntityMapper<KX13.Models.CmsWebFarmServer, KXO.Models.CmsWebFarmServer>, CmsWebFarmMapper>();
        // services.AddTransient<MigrateWebFarmsCommandHandler>();
        
        // pages
        services.AddTransient<MigratePagesCommand>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsTree, KXO.Models.CmsTree>, CmsTreeMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsDocument, KXO.Models.CmsDocument>, CmsDocumentMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsAcl, KXO.Models.CmsAcl>, CmsAclMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath>, CmsPageUrlPathMapper>();

        // media libraries
        services.AddTransient<IEntityMapper<KX13.Models.MediaFile, KXO.Models.MediaFile>, MediaFileMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.MediaLibrary, KXO.Models.MediaLibrary>, MediaLibraryMapper>();
        
        // sites
        services.AddTransient<IEntityMapper<KX13.Models.CmsSite, KXO.Models.CmsSite>, CmsSiteMapper>();
        services.AddTransient<IRequestHandler<MigrateSitesCommand, GenericCommandResult>, MigrateSitesCommandHandler>();
        
        // cms forms
        services.AddTransient<IEntityMapper<KX13.Models.CmsForm, KXO.Models.CmsForm>, CmsFormMapper>();
        services.AddTransient<MigrateFormsCommandHandler>();
        
        services.AddMediatR(typeof(DependencyInjectionExtensions));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestHandlingBehavior<,>));
        // cms data protection
        services.AddTransient<IEntityMapper<KX13.Models.CmsConsent, KXO.Models.CmsConsent>, CmsConsentMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive>, CmsConsentArchiveMapper>();
        services.AddTransient<IEntityMapper<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement>, CmsConsentAgreementMapper>();
        services.AddTransient<MigrateDataProtectionCommandHandler>();

        // IPipelineBehavior 
        return services;
    }
}