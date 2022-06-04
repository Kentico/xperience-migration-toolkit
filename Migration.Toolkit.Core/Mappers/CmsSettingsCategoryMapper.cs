using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsCategoryMapper : IEntityMapper<Migration.Toolkit.KX13.Models.CmsSettingsCategory, Migration.Toolkit.KXO.Models.CmsSettingsCategory>
{
    private readonly ILogger<CmsSettingsCategoryMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsSettingsCategoryMapper(ILogger<CmsSettingsCategoryMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public ModelMappingResult<Migration.Toolkit.KXO.Models.CmsSettingsCategory> Map(Migration.Toolkit.KX13.Models.CmsSettingsCategory? source, Migration.Toolkit.KXO.Models.CmsSettingsCategory? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSettingsCategory>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsSettingsCategory();
            newInstance = true;
        }
        else if (source.CategoryName != target.CategoryName)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsCategory>();
        }

        // map entity
        // target.CategoryId = source.CategoryId;
        target.CategoryDisplayName = source.CategoryDisplayName;
        target.CategoryOrder = source.CategoryOrder;
        target.CategoryName = source.CategoryName;
        target.CategoryParentId = _primaryKeyMappingContext.MapFromSource<CmsCategory>(c => c.CategoryId, source.CategoryParentId);
        target.CategoryIdpath = source.CategoryIdpath;
        target.CategoryLevel = source.CategoryLevel;
        target.CategoryChildCount = source.CategoryChildCount;
        target.CategoryIconPath = source.CategoryIconPath;
        target.CategoryIsGroup = source.CategoryIsGroup;
        target.CategoryIsCustom = source.CategoryIsCustom;
        target.CategoryResourceId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsResource>(r => r.ResourceId, source.CategoryResourceId);
        
        return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsSettingsCategory>(target, newInstance);
    }
}