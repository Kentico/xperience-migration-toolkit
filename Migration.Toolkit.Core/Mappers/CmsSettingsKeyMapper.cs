using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsKeyMapper : IEntityMapper<Migration.Toolkit.KX13.Models.CmsSettingsKey, Migration.Toolkit.KXO.Models.CmsSettingsKey>
{
    private readonly ILogger<CmsSettingsKeyMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsSettingsKeyMapper(ILogger<CmsSettingsKeyMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public ModelMappingResult<Migration.Toolkit.KXO.Models.CmsSettingsKey> Map(Migration.Toolkit.KX13.Models.CmsSettingsKey? source, Migration.Toolkit.KXO.Models.CmsSettingsKey? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSettingsKey>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsSettingsKey();
            newInstance = true;
        }
        else if (CmsSettingsKeyKey.From(source) != CmsSettingsKeyKey.From(target))
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsKey>();
        }

        // map entity
        // source.KeyId = target.KeyId;
        target.KeyName = source.KeyName;
        target.KeyDisplayName = source.KeyDisplayName;
        target.KeyDescription = source.KeyDescription;
        target.KeyValue = source.KeyValue;
        target.KeyType = source.KeyType;
        target.KeyCategoryId = source.KeyCategoryId;
        target.SiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(s => s.SiteId, source.SiteId);
        target.KeyGuid = source.KeyGuid;
        target.KeyLastModified = source.KeyLastModified;
        target.KeyOrder = source.KeyOrder;
        target.KeyDefaultValue = source.KeyDefaultValue;
        target.KeyValidation = source.KeyValidation;
        target.KeyEditingControlPath = source.KeyEditingControlPath;
        target.KeyIsGlobal = source.KeyIsGlobal;
        target.KeyIsCustom = source.KeyIsCustom;
        target.KeyIsHidden = source.KeyIsHidden;
        target.KeyFormControlSettings = source.KeyFormControlSettings;
        target.KeyExplanationText = source.KeyExplanationText;

        return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsSettingsKey>(target, newInstance);
    }
}

public record CmsSettingsKeyKey(string KeyName, int? SiteId, Guid KeyGuid)
{
    public override string ToString()
    {
        return $"KN={KeyName.PadLeft(60,' ')} SID={SiteId} G={KeyGuid}";
    }

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KX13.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId, cmsSettingsKey.KeyGuid);

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KXO.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId, cmsSettingsKey.KeyGuid);
    
    public static CmsSettingsKeyKey From(string? keyName, int? siteId, Guid keyGuid)
    {
        ArgumentNullException.ThrowIfNull(keyName);
        
        return new(keyName, siteId, keyGuid);
    }
}