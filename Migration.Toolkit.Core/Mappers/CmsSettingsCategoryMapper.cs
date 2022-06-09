using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsCategoryMapper : IEntityMapper<Migration.Toolkit.KX13.Models.CmsSettingsCategory, Migration.Toolkit.KXO.Models.CmsSettingsCategory>
{
    private readonly ILogger<CmsSettingsCategoryMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IEntityMapper<KX13.Models.CmsResource, KXO.Models.CmsResource> _cmsResourceMapper;

    public CmsSettingsCategoryMapper(ILogger<CmsSettingsCategoryMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext, IEntityMapper<Migration.Toolkit.KX13.Models.CmsResource, Migration.Toolkit.KXO.Models.CmsResource> cmsResourceMapper)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _cmsResourceMapper = cmsResourceMapper;
    }

    public IModelMappingResult<Migration.Toolkit.KXO.Models.CmsSettingsCategory> Map(Migration.Toolkit.KX13.Models.CmsSettingsCategory? source, Migration.Toolkit.KXO.Models.CmsSettingsCategory? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSettingsCategory>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsSettingsCategory();
            newInstance = true;
        }
        // no category guid to match on...
        // else if (source.CategoryName != target.CategoryName)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsCategory>();
        // }

        // map entity
        // target.CategoryId = source.CategoryId;
        target.CategoryDisplayName = source.CategoryDisplayName;
        target.CategoryOrder = source.CategoryOrder;
        target.CategoryName = source.CategoryName;
        
        var aggregatedResult = new AggregatedResult<Migration.Toolkit.KXO.Models.CmsSettingsCategory>(target, newInstance);

        if (source.CategoryResource != null)
        {
            switch (_cmsResourceMapper.Map(source.CategoryResource, target.CategoryResource))
            {
                case { Success: true } result:
                {
                    target.CategoryResource = result.Item;
                    break;
                }
                case { Success: false } result:
                {
                    aggregatedResult.AddResult(result);
                    return aggregatedResult.Log(_logger);
                }
            }
        }
        else
        {
            target.CategoryResourceId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsResource>(r => r.ResourceId, source.CategoryResourceId);
        }
        
        if (source.CategoryParent != null)
        {
            switch (Map(source.CategoryParent, target.CategoryParent))
            {
                case { Success: true } result:
                {
                    target.CategoryParent = result.Item;
                    break;
                }
                case { Success: false } result:
                {
                    aggregatedResult.AddResult(result);
                    return aggregatedResult.Log(_logger);
                }
            }
        }
        else
        {
            target.CategoryParentId = _primaryKeyMappingContext.MapFromSource<CmsCategory>(c => c.CategoryId, source.CategoryParentId);
        }
        
        target.CategoryIdpath = source.CategoryIdpath;
        target.CategoryLevel = source.CategoryLevel;
        target.CategoryChildCount = source.CategoryChildCount;
        target.CategoryIconPath = source.CategoryIconPath;
        target.CategoryIsGroup = source.CategoryIsGroup;
        target.CategoryIsCustom = source.CategoryIsCustom;

        return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsSettingsCategory>(target, newInstance).Log(_logger);
    }
}