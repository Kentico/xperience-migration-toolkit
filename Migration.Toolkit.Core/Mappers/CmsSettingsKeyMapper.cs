using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsKeyMapper : IEntityMapper<Migration.Toolkit.KX13.Models.CmsSettingsKey, Migration.Toolkit.KXO.Models.CmsSettingsKey>
{
    private readonly ILogger<CmsSettingsKeyMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IEntityMapper<K13M.CmsSettingsCategory, KXO.Models.CmsSettingsCategory> _cmsCategoryMapper;

    public CmsSettingsKeyMapper(ILogger<CmsSettingsKeyMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext,
        IEntityMapper<K13M.CmsSettingsCategory, KXO.Models.CmsSettingsCategory> cmsCategoryMapper)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _cmsCategoryMapper = cmsCategoryMapper;
    }

    public IModelMappingResult<Migration.Toolkit.KXO.Models.CmsSettingsKey> Map(Migration.Toolkit.KX13.Models.CmsSettingsKey? source, Migration.Toolkit.KXO.Models.CmsSettingsKey? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSettingsKey>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsSettingsKey();
            newInstance = true;
        }
        else if (CmsSettingsKeyKey.From(source.KeyName, _primaryKeyMappingContext.MapFromSource<K13M.CmsSite>(s=>s.SiteId, source.SiteId)) != CmsSettingsKeyKey.From(target.KeyName, target.SiteId))
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsKey>().Log(_logger);
        }

        // map entity
        // source.KeyId = target.KeyId;
        
        if (newInstance)
        {
            target.KeyName = source.KeyName;
            target.KeyDisplayName = source.KeyDisplayName;
            target.KeyDescription = source.KeyDescription;
            target.KeyType = source.KeyType;
            target.KeyGuid = source.KeyGuid;
            target.KeyOrder = source.KeyOrder;
            target.KeyDefaultValue = source.KeyDefaultValue;
            target.KeyValidation = source.KeyValidation;
            target.KeyEditingControlPath = source.KeyEditingControlPath;
            target.KeyIsGlobal = source.KeyIsGlobal;
            target.KeyIsCustom = source.KeyIsCustom;
            target.KeyFormControlSettings = source.KeyFormControlSettings;
            target.KeyExplanationText = source.KeyExplanationText;
        }
        
        target.KeyValue = source.KeyValue;
        
        // target.KeyCategoryId = source.KeyCategoryId; - mapped using EF
        target.SiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(s => s.SiteId, source.SiteId);
        target.KeyLastModified = source.KeyLastModified;
        // target.KeyIsHidden = source.KeyIsHidden; - not mapped / internal
        
        var aggregatedResult = new AggregatedResult<Migration.Toolkit.KXO.Models.CmsSettingsKey>(target, newInstance);
        if (source.KeyCategory != null)
        {
            switch (_cmsCategoryMapper.Map(source.KeyCategory, target.KeyCategory))
            {
                case { Success: true } result:
                {
                    target.KeyCategory = result.Item;
                    break;
                }
                case { Success: false } result:
                {
                    aggregatedResult.AddResult(result);
                    return aggregatedResult.Log(_logger);
                }
            }
        }

        // return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsSettingsKey>(target, newInstance);
        return aggregatedResult.Log(_logger);
    }
}

public record CmsSettingsKeyKey(string KeyName, int? SiteId)//, Guid KeyGuid)
{
    public override string ToString()
    {
        return $"KN={KeyName.PadLeft(60,' ')} SID={SiteId}";
    }

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KX13.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId);//, cmsSettingsKey.KeyGuid);

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KXO.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId);// , cmsSettingsKey.KeyGuid);

    public static CmsSettingsKeyKey From(string? keyName, int? siteId) //, Guid keyGuid)
    {
        ArgumentNullException.ThrowIfNull(keyName);

        return new(keyName, siteId); //, keyGuid);
    }
}