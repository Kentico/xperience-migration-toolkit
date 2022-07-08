using System.Collections.Immutable;
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
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

// TODO tk: 2022-06-01 Q - dle jakého klíče přenášet kategorie settings? jen pro klíče, které budou přeneseny do cílové instance?

public class MigrateSettingKeysCommandHandler: IRequestHandler<MigrateSettingKeysCommand, MigrateSettingsKeysResult>, IDisposable
{
    private readonly ILogger<MigrateSettingKeysCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly IEntityMapper<KX13.Models.CmsSettingsKey, KXO.Models.CmsSettingsKey> _mapper;
    private readonly IEntityMapper<CmsSettingsCategory, KXO.Models.CmsSettingsCategory> _cmsSettingsCategoryMapper;
    private readonly IEntityMapper<KX13.Models.CmsResource, KXO.Models.CmsResource> _cmsResourceMapper;
    
    private KxoContext _kxoContext;

    public MigrateSettingKeysCommandHandler(
        ILogger<MigrateSettingKeysCommandHandler> logger,
        IEntityMapper<KX13.Models.CmsSettingsKey, KXO.Models.CmsSettingsKey> mapper,
        IEntityMapper<KX13.Models.CmsSettingsCategory, KXO.Models.CmsSettingsCategory> cmsSettingsCategoryMapper,
        IEntityMapper<KX13.Models.CmsResource, KXO.Models.CmsResource> cmsResourceMapper,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        ToolkitConfiguration toolkitConfiguration,
        IMigrationProtocol migrationProtocol
        )
    {
        _logger = logger;
        _mapper = mapper;
        _cmsSettingsCategoryMapper = cmsSettingsCategoryMapper;
        _cmsResourceMapper = cmsResourceMapper;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _toolkitConfiguration = toolkitConfiguration;
        _migrationProtocol = migrationProtocol;
        _kxoContext = _kxoContextFactory.CreateDbContext();
    }
   
    // private async Task RequireCmsResourceDependencyAsync(KX13.Models.CmsResource? kx13CmsResource, CancellationToken cancellationToken)
    // {
    //     _migrationProtocol.FetchedSource(kx13CmsResource);
    //     
    //     if (kx13CmsResource != null)
    //     {
    //         _logger.LogTrace("Migrating dependency for {cmsSettingsCategory} on {cmsResourceString} in target instance", nameof(KX13.Models.CmsSettingsCategory), nameof(KX13.Models.CmsResource));
    //
    //         var kxoCmsResource = await _kxoContext.CmsResources.FirstOrDefaultAsync(r => r.ResourceName == kx13CmsResource.ResourceName, cancellationToken: cancellationToken);
    //         
    //         _migrationProtocol.FetchedTarget(kxoCmsResource);
    //
    //         var mapped = _cmsResourceMapper.Map(kx13CmsResource, kxoCmsResource);
    //         _migrationProtocol.MappedTarget(mapped);
    //         
    //         switch (mapped)
    //         {
    //             case ModelMappingSuccess<KXO.Models.CmsResource>(var cmsResource, var newInstance):
    //                 ArgumentNullException.ThrowIfNull(cmsResource, nameof(cmsResource));
    //                 
    //                 if (newInstance)
    //                 {
    //                     _kxoContext.CmsResources.Add(cmsResource);
    //                 }
    //                 else
    //                 {
    //                     _kxoContext.CmsResources.Update(cmsResource);
    //                 }
    //
    //                 await _kxoContext.SaveChangesAsync(cancellationToken);
    //                 _migrationProtocol.Success(kx13CmsResource, kxoCmsResource, mapped);
    //                 
    //                 _logger.LogInformation(newInstance
    //                     ? $"CmsResource: {cmsResource.ResourceName} was inserted."
    //                     : $"CmsResource: {cmsResource.ResourceName} was updated.");
    //
    //                 _primaryKeyMappingContext.SetMapping<KX13.Models.CmsResource>(r => r.ResourceId, kx13CmsResource.ResourceId, cmsResource.ResourceId);
    //
    //                 return;
    //             default:
    //                 // TODO tk: 2022-05-18 mapping failed but already logged, do something..
    //                 break;
    //         }
    //     }
    // }

    public async Task<MigrateSettingsKeysResult> Handle(MigrateSettingKeysCommand request, CancellationToken cancellationToken)
    {
        var entityConfiguration = _toolkitConfiguration.EntityConfigurations.GetEntityConfiguration<KX13.Models.CmsSettingsKey>();
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        // not needed right naw as category will be migrated when needed 
        // await RequireMigratedCmsSettingsCategories(kx13Context, cancellationToken);
        
        _logger.LogInformation($"CmsSettingsKey synchronization starting");
        var cmsSettingsKeys = kx13Context.CmsSettingsKeys
                // .Include(sk => sk.KeyCategory.CategoryResource)
                // .Include(sk => sk.KeyCategory.CategoryParent.CategoryResource)
                // .Include(sk => sk.KeyCategory.CategoryParent.CategoryParent.CategoryResource)
                .Where(csk => migratedSiteIds.Contains(csk.SiteId) || csk.SiteId == null)
                .AsNoTrackingWithIdentityResolution()
            // .Where(k => k.KeyIsCustom == true)
            ;

        // var presentSettingsKeyNames = _kxoContext.CmsSettingsKeys.Select(x => x.KeyName).ToImmutableHashSet();
        
        foreach (var kx13CmsSettingsKey in cmsSettingsKeys)
        {
            // if (!presentSettingsKeyNames.Contains(kx13CmsSettingsKey.KeyName) && kx13CmsSettingsKey.KeyIsCustom.GetValueOrDefault(false))
            // {
            //     _logger.LogWarning("Setting with key '{key}' is not supported.", kx13CmsSettingsKey.KeyName);
            //     // TODO tk: 2022-06-01 protocol needs attention?
            //     continue;
            // }
            
            _migrationProtocol.FetchedSource(kx13CmsSettingsKey);

            var kxoGlobalSettingsKey = GetKxoSettingsKey(kx13CmsSettingsKey, null);
            
            var canBeMigrated = !kxoGlobalSettingsKey?.KeyIsHidden ?? false;
            var kxoCmsSettingsKey = kx13CmsSettingsKey.SiteId is null ? kxoGlobalSettingsKey : GetKxoSettingsKey(kx13CmsSettingsKey, kx13CmsSettingsKey.SiteId);

            if (!canBeMigrated)
            {
                _logger.LogWarning("Setting with key '{Key}' is currently not supported for migration", kx13CmsSettingsKey.KeyName);
                _migrationProtocol.Append(
                    HandbookReferences
                        .NotCurrentlySupportedSkip<SettingsKeyInfo>()
                        .WithId(nameof(kx13CmsSettingsKey.KeyId), kx13CmsSettingsKey.KeyId)
                        .WithData(new
                        {
                            kx13CmsSettingsKey.KeyName,
                            kx13CmsSettingsKey.SiteId,
                            kx13CmsSettingsKey.KeyGuid
                        })
                );
                continue;
            }
            // else if(kxoCmsSettingsKey == null)
            // {
            //     _logger.LogWarning("Setting with key '{key}' not exists in target database (creating new keys with tool is not currently supported).", kx13CmsSettingsKey.KeyName);
            //     // TODO tk: 2022-06-01 protocol needs attention?
            //     continue;
            // }
            // else
            // {
            _migrationProtocol.FetchedTarget(kxoCmsSettingsKey);
            // }

            if (entityConfiguration.ExcludeCodeNames.Contains(kx13CmsSettingsKey.KeyName))
            {
                _migrationProtocol.Warning(HandbookReferences.CmsSettingsKeyExclusionListSkip, kx13CmsSettingsKey);
                _logger.LogWarning("KeyName {keyName} is excluded => skipping", kx13CmsSettingsKey.KeyName);
                continue;
            }

            var mapped = _mapper.Map(kx13CmsSettingsKey, kxoCmsSettingsKey);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success: true } result)
            {
                ArgumentNullException.ThrowIfNull(result.Item, nameof(result.Item));

                if (result.NewInstance)
                {
                    _kxoContext.CmsSettingsKeys.Add(result.Item);
                }
                else
                {
                    _kxoContext.CmsSettingsKeys.Update(result.Item);
                }

                await _kxoContext.SaveChangesAsync(cancellationToken);
                
                // await _kxoContext.DisposeAsync();
                // _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);
                
                _migrationProtocol.Success(kx13CmsSettingsKey, kxoCmsSettingsKey, mapped);

                _logger.LogInformation(result.NewInstance
                    ? $"CmsSettingsKey: {result.Item.KeyName} was inserted."
                    : $"CmsSettingsKey: {result.Item.KeyName} was updated.");
            }
        }

        return new MigrateSettingsKeysResult();
    }

    private KXO.Models.CmsSettingsKey? GetKxoSettingsKey(KX13.Models.CmsSettingsKey kx13CmsSettingsKey, int? siteId)
    {
        using var kxoContext = _kxoContextFactory.CreateDbContext();
        
        return kxoContext.CmsSettingsKeys
            // .Include(sk => sk.KeyCategory.CategoryResource)
            // .Include(sk => sk.KeyCategory.CategoryParent.CategoryResource)
            // .Include(sk => sk.KeyCategory.CategoryParent.CategoryParent.CategoryResource)
            .SingleOrDefault(k => k.KeyName == kx13CmsSettingsKey.KeyName && k.SiteId == siteId);
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}