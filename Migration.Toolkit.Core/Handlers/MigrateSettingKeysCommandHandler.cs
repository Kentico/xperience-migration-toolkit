using System.Collections.Immutable;
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
   
    private async Task RequireCmsResourceDependencyAsync(KX13.Models.CmsResource? kx13CmsResource, CancellationToken cancellationToken)
    {
        _migrationProtocol.FetchedSource(kx13CmsResource);
        
        if (kx13CmsResource != null)
        {
            _logger.LogTrace("Migrating dependency for {cmsSettingsCategory} on {cmsResourceString} in target instance", nameof(KX13.Models.CmsSettingsCategory), nameof(KX13.Models.CmsResource));

            var kxoCmsResource = await _kxoContext.CmsResources.FirstOrDefaultAsync(r => r.ResourceName == kx13CmsResource.ResourceName, cancellationToken: cancellationToken);
            
            _migrationProtocol.FetchedTarget(kxoCmsResource);

            var mapped = _cmsResourceMapper.Map(kx13CmsResource, kxoCmsResource);
            mapped.LogResult(_logger);
            _migrationProtocol.MappedTarget(mapped);
            
            switch (mapped)
            {
                case ModelMappingSuccess<KXO.Models.CmsResource>(var cmsResource, var newInstance):
                    ArgumentNullException.ThrowIfNull(cmsResource, nameof(cmsResource));
                    
                    if (newInstance)
                    {
                        _kxoContext.CmsResources.Add(cmsResource);
                    }
                    else
                    {
                        _kxoContext.CmsResources.Update(cmsResource);
                    }

                    await _kxoContext.SaveChangesAsync(cancellationToken);
                    _migrationProtocol.Success(kx13CmsResource, kxoCmsResource, mapped);
                    
                    _logger.LogInformation(newInstance
                        ? $"CmsResource: {cmsResource.ResourceName} was inserted."
                        : $"CmsResource: {cmsResource.ResourceName} was updated.");

                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsResource>(r => r.ResourceId, kx13CmsResource.ResourceId, cmsResource.ResourceId);

                    return;
                default:
                    // TODO tk: 2022-05-18 mapping failed but already logged, do something..
                    break;
            }
        }
    }

    private async Task RequireMigratedCmsSettingsCategories(KX13Context kx13Context, CancellationToken cancellationToken)
    {
        var kx13CmsSettingsCategories = kx13Context.CmsSettingsCategories
            .Include(sc => sc.CategoryResource)
            .OrderBy(sc => sc.CategoryLevel)
            .ThenBy(sc => sc.CategoryParentId);

        foreach (var kx13CmsSettingsCategory in kx13CmsSettingsCategories)
        {
            _migrationProtocol.FetchedSource(kx13CmsSettingsCategory);
            
            var kxoCmsSettingsCategory = _kxoContext.CmsSettingsCategories.FirstOrDefault(k => k.CategoryName == kx13CmsSettingsCategory.CategoryName);
            _migrationProtocol.FetchedTarget(kxoCmsSettingsCategory);

            _logger.LogTrace("Check {cmsSettingsCategory} dependency on {cmsResourceString}", nameof(KX13.Models.CmsSettingsCategory), nameof(KX13.Models.CmsResourceString));
            await RequireCmsResourceDependencyAsync(kx13CmsSettingsCategory.CategoryResource, cancellationToken);
            
            var mapped = _cmsSettingsCategoryMapper.Map(kx13CmsSettingsCategory, kxoCmsSettingsCategory);
            mapped.LogResult(_logger);
            _migrationProtocol.MappedTarget(mapped);
            
            switch (mapped)
            {
                case ModelMappingSuccess<KXO.Models.CmsSettingsCategory>(var result, var newInstance):
                    ArgumentNullException.ThrowIfNull(result, nameof(result));

                    if (kx13CmsSettingsCategory.CategoryResourceId is int categoryResourceId)
                    {
                        result.CategoryResourceId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsResource>(r => r.ResourceId, categoryResourceId);
                    }
                    
                    if (newInstance)
                    {
                        _kxoContext.CmsSettingsCategories.Add(result);
                    }
                    else
                    {
                        _kxoContext.CmsSettingsCategories.Update(result);
                    }

                    await _kxoContext.SaveChangesAsync(cancellationToken);
                    
                    _migrationProtocol.Success(kx13CmsSettingsCategory, kxoCmsSettingsCategory, mapped);
                    
                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsCategory>(c => c.CategoryId, kx13CmsSettingsCategory.CategoryId, result.CategoryId);

                    _logger.LogInformation(newInstance
                        ? $"CmsSettingsCategory: {result.CategoryName} was inserted."
                        : $"CmsSettingsCategory: {result.CategoryName} was updated.");

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }
    }
    
    public async Task<MigrateSettingsKeysResult> Handle(MigrateSettingKeysCommand request, CancellationToken cancellationToken)
    {
        var entityConfiguration = _toolkitConfiguration.EntityConfigurations.GetEntityConfiguration<KX13.Models.CmsSettingsKey>();
        var explicitSiteIdMapping = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        await RequireMigratedCmsSettingsCategories(kx13Context, cancellationToken);

        _logger.LogInformation($"CmsSettingsKey synchronization starting");
        var cmsSettingsKeys = kx13Context.CmsSettingsKeys
                .Where(csk=> explicitSiteIdMapping.Contains(csk.SiteId) || csk.SiteId == null)
            // .Where(k => k.KeyIsCustom == true)
            ;

        var presentSettingsKeyNames = _kxoContext.CmsSettingsKeys.Select(x => x.KeyName).ToImmutableHashSet();
        
        foreach (var kx13CmsSettingsKey in cmsSettingsKeys)
        {
            if (!presentSettingsKeyNames.Contains(kx13CmsSettingsKey.KeyName) && kx13CmsSettingsKey.KeyIsCustom == false)
            {
                _logger.LogWarning("Setting with key '{key}' is no longer supported.", kx13CmsSettingsKey.KeyName);
                // TODO tk: 2022-06-01 protocol needs attention?
                continue;
            }
            
            _migrationProtocol.FetchedSource(kx13CmsSettingsKey);
            
            var kxoCmsSettingsKey = _kxoContext.CmsSettingsKeys.FirstOrDefault(k => k.KeyName == kx13CmsSettingsKey.KeyName && k.SiteId == kx13CmsSettingsKey.SiteId);
            _migrationProtocol.FetchedTarget(kxoCmsSettingsKey);

            if (entityConfiguration.ExcludeCodeNames.Contains(kx13CmsSettingsKey.KeyName))
            {
                _migrationProtocol.Warning(HandbookReferences.CmsSettingsKeyExclusionListSkip, kx13CmsSettingsKey);
                _logger.LogWarning("KeyName {keyName} is excluded => skipping", kx13CmsSettingsKey.KeyName);
                continue;
            }
            
            var mapped = _mapper.Map(kx13CmsSettingsKey, kxoCmsSettingsKey);
            mapped.LogResult(_logger);
            _migrationProtocol.MappedTarget(mapped);
            
            switch (mapped)
            {
                case ModelMappingSuccess<KXO.Models.CmsSettingsKey>(var cmsSettingsKeyResult, var newInstance):
                    ArgumentNullException.ThrowIfNull(cmsSettingsKeyResult, nameof(cmsSettingsKeyResult));
                    
                    if (kx13CmsSettingsKey.KeyCategoryId is int sourceCategoryId)
                    {
                        mapped.Item!.KeyCategoryId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsCategory>(c => c.CategoryId, sourceCategoryId);
                    }
                    
                    if (newInstance)
                    {
                        _kxoContext.CmsSettingsKeys.Add(cmsSettingsKeyResult);
                    }
                    else
                    {
                        _kxoContext.CmsSettingsKeys.Update(cmsSettingsKeyResult);
                    }

                    await _kxoContext.SaveChangesAsync(cancellationToken);
                    
                    _migrationProtocol.Success(kx13CmsSettingsKey, kxoCmsSettingsKey, mapped);

                    _logger.LogInformation(newInstance
                        ? $"CmsSettingsKey: {cmsSettingsKeyResult.KeyName} was inserted."
                        : $"CmsSettingsKey: {cmsSettingsKeyResult.KeyName} was updated.");

                    // kxoContext.SaveChangesWithIdentityInsert<KXO.Models.CmsClass>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }

        return new MigrateSettingsKeysResult();
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}