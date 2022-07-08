using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CMS.DataEngine;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KX13.Models;
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigratePageTypesCommandHandler : IRequestHandler<MigratePageTypesCommand, MigratePageTypesResult>
{
    private readonly ILogger<MigratePageTypesCommandHandler> _logger;
    private readonly IEntityMapper<CmsClass, DataClassInfo> _dataClassMapper;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly KxoClassFacade _kxoClassFacade;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly ToolkitConfiguration _toolkitConfiguration;

    public MigratePageTypesCommandHandler(
        ILogger<MigratePageTypesCommandHandler> logger,
        IEntityMapper<KX13.Models.CmsClass, DataClassInfo> dataClassMapper,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        KxoClassFacade kxoClassFacade,
        IMigrationProtocol migrationProtocol,
        ToolkitConfiguration toolkitConfiguration
    )
    {
        _logger = logger;
        _dataClassMapper = dataClassMapper;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _kxoClassFacade = kxoClassFacade;
        _migrationProtocol = migrationProtocol;
        _toolkitConfiguration = toolkitConfiguration;
    }

    public async Task<MigratePageTypesResult> Handle(MigratePageTypesCommand request, CancellationToken cancellationToken)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        Debug.Assert(_toolkitConfiguration.EntityConfigurations != null, "_toolkitConfiguration.EntityConfigurations != null");
        var entityConfiguration = _toolkitConfiguration.EntityConfigurations.GetEntityConfiguration<KX13.Models.CmsClass>();
        
        var siteIdExplicitMapping = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId);
        var migratedSiteIds = siteIdExplicitMapping.Keys.ToList();

        var cmsClassesDocumentTypes = kx13Context.CmsClasses.Include(c => c.Sites)
                .Where(x => x.ClassIsDocumentType).OrderBy(x => x.ClassId).AsEnumerable()
            ;

        foreach (var kx13Class in cmsClassesDocumentTypes)
        {
            if (!kx13Class.Sites.Any(s => migratedSiteIds.Contains(s.SiteId)))
            {
                // skip classes not included in current site
                _logger.LogTrace("Class {ClassName} skipped, no site mapping", kx13Class.ClassName);
                continue;
            }
            
            _migrationProtocol.FetchedSource(kx13Class);

            if (entityConfiguration.ExcludeCodeNames.Contains(kx13Class.ClassName, StringComparer.InvariantCultureIgnoreCase))
            {
                _migrationProtocol.Warning(HandbookReferences.EntityExplicitlyExcludedByCodeName(kx13Class.ClassName, "PageType"), kx13Class);
                _logger.LogWarning("CmsClass: {kx13Class.ClassName} was skipped => it is explicitly excluded in configuration.", kx13Class.ClassName);
                continue;    
            }
            
            if (kx13Class.ClassName == "CMS.Root")
            {
                _migrationProtocol.Warning(HandbookReferences.CmsClassCmsRootClassTypeSkip, kx13Class);
                _logger.LogWarning($"CmsClass: {kx13Class.ClassName} was skipped => CMS.Root cannot be migrated.");
                continue;
            }

            // nekontrolujeme
            // if (kx13Class.ClassConnectionString?.ToLowerInvariant() != "cmsconnectionstring" &&
            //     kx13Class.ClassConnectionString != _toolkitConfiguration.SourceConnectionString &&
            //     string.IsNullOrWhiteSpace(kx13Class.ClassConnectionString))
            // {
            //     _migrationProtocol.Warning(HandbookReferences.CmsClassClassConnectionStringIsDifferent, kx13Class);
            //     _logger.LogWarning(
            //         $"CmsClass: {kx13Class.ClassName} => ClassConnectionString is different from source connection string needs attention!");
            // }

            var kxoDataClass = _kxoClassFacade.GetClass(kx13Class.ClassGuid);
            _migrationProtocol.FetchedTarget(kxoDataClass);

            var dataClassId = SaveUsingKxoApi(kx13Class, kxoDataClass);
            // await SaveUsingEntityFramework(cancellationToken, kx13CmsClassesDocumentType, kxoCmsClass, kxoContext);

            if (dataClassId is int dcId)
            {
                MigrateClassSiteMappings(siteIdExplicitMapping, dcId, dataClassId, kx13Class);
            }
        }

        return new MigratePageTypesResult();
    }

    private void MigrateClassSiteMappings(Dictionary<int?, int?> siteIdExplicitMapping, int dcId, [DisallowNull] int? dataClassId, CmsClass kx13Class)
    {
        foreach (var (sourceSiteId, targetSiteId) in siteIdExplicitMapping)
        {
            if (targetSiteId is not int tsId)
            {
                continue;
            }

            try
            {
                var classSiteInfo = ClassSiteInfo.New();
                classSiteInfo.ClassID = dcId;
                classSiteInfo.SiteID = tsId;
                ClassSiteInfoProvider.ProviderObject.Set(classSiteInfo);

                _migrationProtocol.Success(new { dataClassId, targetSiteId }, classSiteInfo, null);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to create target instance");
                _migrationProtocol.Append(HandbookReferences
                    .ErrorCreatingTargetInstance<ClassSiteInfo>(exception)
                    .NeedsManualAction()
                    .WithData(new
                    {
                        exception,
                        sourceSiteId,
                        targetSiteId,
                        dataClassId,
                        sourceClassId = kx13Class.ClassId
                    })
                );
            }
        }
    }

    private int? SaveUsingKxoApi(CmsClass kx13Class, DataClassInfo kxoDataClass)
    {
        var mapped = _dataClassMapper.Map(kx13Class, kxoDataClass);
        _migrationProtocol.MappedTarget(mapped);

        try
        {
            switch (mapped)
            {
                case { Success : true } result:
                {
                    var (dataClassInfo, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(dataClassInfo, nameof(dataClassInfo));

                    _kxoClassFacade.SetClass(dataClassInfo);

                    _migrationProtocol.Success(kx13Class, dataClassInfo, mapped);

                    _logger.LogInformation(newInstance
                        ? $"CmsClass: {dataClassInfo.ClassName} was inserted."
                        : $"CmsClass: {dataClassInfo.ClassName} was updated.");

                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsClass>(
                        r => r.ClassId,
                        kx13Class.ClassId,
                        dataClassInfo.ClassID
                    );

                    return dataClassInfo.ClassID;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while saving page type {className}", kx13Class.ClassName);
        }

        return null;
    }

    // private async Task SaveUsingEntityFramework(CancellationToken cancellationToken, CmsClass kx13CmsClassesDocumentType,
    //     KXO.Models.CmsClass? kxoCmsClass, KxoContext kxoContext)
    // {
    //     var mapped = _mapper.Map(kx13CmsClassesDocumentType, kxoCmsClass);
    //     _migrationProtocol.MappedTarget(mapped);
    //
    //     switch (mapped)
    //     {
    //         case { Success : true } result:
    //         {
    //             var (cmsClass, newInstance) = result;
    //             ArgumentNullException.ThrowIfNull(cmsClass, nameof(cmsClass));
    //
    //             if (newInstance)
    //             {
    //                 kxoContext.CmsClasses.Add(cmsClass);
    //             }
    //             else
    //             {
    //                 kxoContext.CmsClasses.Update(cmsClass);
    //             }
    //
    //             await kxoContext.SaveChangesAsync(cancellationToken); // TODO tk: 2022-05-18 context needs to be disposed/recreated after error
    //
    //             _migrationProtocol.Success(kx13CmsClassesDocumentType, kxoCmsClass, mapped);
    //
    //             _logger.LogInformation(newInstance
    //                 ? $"CmsClass: {cmsClass.ClassName} was inserted."
    //                 : $"CmsClass: {cmsClass.ClassName} was updated.");
    //
    //             // kxoContext.SaveChangesWithIdentityInsert<KXO.Models.CmsClass>();
    //             break;
    //         }
    //         default:
    //             throw new ArgumentOutOfRangeException(nameof(mapped));
    //     }
    // }
}