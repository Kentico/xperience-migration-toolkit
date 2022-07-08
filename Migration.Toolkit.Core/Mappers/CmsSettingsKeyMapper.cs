using CMS.Helpers;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsKeyMapper : EntityMapperBase<Migration.Toolkit.KX13.Models.CmsSettingsKey, Migration.Toolkit.KXO.Models.CmsSettingsKey>
{
    private readonly IEntityMapper<KX13.Models.CmsSettingsCategory, KXO.Models.CmsSettingsCategory> _cmsCategoryMapper;

    public CmsSettingsKeyMapper(ILogger<CmsSettingsKeyMapper> logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol,
        IEntityMapper<CmsSettingsCategory, KXO.Models.CmsSettingsCategory> cmsCategoryMapper) : base(logger, pkContext, protocol)
    {
        _cmsCategoryMapper = cmsCategoryMapper;
    }

    protected override KXO.Models.CmsSettingsKey? CreateNewInstance(KX13.Models.CmsSettingsKey source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override KXO.Models.CmsSettingsKey MapInternal(KX13.Models.CmsSettingsKey source, KXO.Models.CmsSettingsKey target, bool newInstance,
        MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source is null)
        // {
        //     _logger.LogTrace("Source entity is not defined.");
        //     return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsSettingsKey>().Log(_logger);
        // }
        //
        // var newInstance = false;
        // if (target is null)
        // {
        //     _logger.LogTrace("Null target supplied, creating new instance.");
        //     target = new Migration.Toolkit.KXO.Models.CmsSettingsKey();
        //     newInstance = true;
        // }
        // else if (CmsSettingsKeyKey.From(source.KeyName, _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(s=>s.SiteId, source.SiteId)) != CmsSettingsKeyKey.From(target.KeyName, target.SiteId))
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsKey>().Log(_logger);
        // }

        // map entity
        // source.KeyId = target.KeyId;

        if (newInstance)
        {
            target.KeyName = source.KeyName;
            target.KeyDisplayName = source.KeyDisplayName;
            target.KeyDescription = source.KeyDescription;
            target.KeyType = source.KeyType;
            target.KeyGuid = source.KeyGuid;
            // target.KeyOrder = source.KeyOrder;
            target.KeyDefaultValue = source.KeyDefaultValue;
            target.KeyValidation = source.KeyValidation;
            target.KeyEditingControlPath = source.KeyEditingControlPath;
            // target.KeyIsGlobal = source.KeyIsGlobal;
            // target.KeyIsCustom = source.KeyIsCustom;
            target.KeyFormControlSettings = source.KeyFormControlSettings;
            target.KeyExplanationText = source.KeyExplanationText;    
        }
        else
        {
            target.KeyName = source.KeyName;
            // target.KeyDisplayName = source.KeyDisplayName;
            target.KeyDescription = source.KeyDescription;
            target.KeyType = source.KeyType;
            target.KeyGuid = source.KeyGuid;
            // target.KeyOrder = source.KeyOrder;
            target.KeyDefaultValue = source.KeyDefaultValue;
            target.KeyValidation = source.KeyValidation;
            target.KeyEditingControlPath = source.KeyEditingControlPath;
            // target.KeyIsGlobal = source.KeyIsGlobal;
            // target.KeyIsCustom = source.KeyIsCustom;
            target.KeyFormControlSettings = source.KeyFormControlSettings;
            target.KeyExplanationText = source.KeyExplanationText;
        }

        switch (source.KeyName)
        {
            case "CMSDefaultUserID":
            {
                target.KeyValue = int.TryParse(source.KeyValue, out var cmsDefaultUserId)
                    ? mappingHelper.TranslateRequiredId<CmsUser>(u => u.UserId, cmsDefaultUserId, out var targetCmsDefaultUserId)
                        ? targetCmsDefaultUserId.ToString()
                        : source.KeyValue
                    : source.KeyValue;
                break;
            }
            default:
                target.KeyValue = source.KeyValue;
                break;
        }

        // target.KeyCategoryId = ;
        if (mappingHelper.TranslateId<KX13.Models.CmsSite>(s => s.SiteId, source.SiteId, out var siteId))
        {
            target.SiteId = siteId;
        }
        target.KeyLastModified = source.KeyLastModified;
        // target.KeyIsHidden = source.KeyIsHidden; - not mapped / internal

        // if (source.KeyCategory != null)
        // {
        //     switch (_cmsCategoryMapper.Map(source.KeyCategory, target.KeyCategory))
        //     {
        //         case { Success: true } result:
        //         {
        //             target.KeyCategory = result.Item;
        //             break;
        //         }
        //         case { Success: false } result:
        //         {
        //             addFailure(new MapperResultFailure<KXO.Models.CmsSettingsKey>(result.HandbookReference));
        //             break;
        //         }
        //     }
        // }
        
        return target;
    }
}

public record CmsSettingsKeyKey(string KeyName, int? SiteId) //, Guid KeyGuid)
{
    public override string ToString()
    {
        return $"KN={KeyName.PadLeft(60, ' ')} SID={SiteId}";
    }

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KX13.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId); //, cmsSettingsKey.KeyGuid);

    public static CmsSettingsKeyKey? From(Migration.Toolkit.KXO.Models.CmsSettingsKey? cmsSettingsKey) =>
        cmsSettingsKey == null ? null : new(cmsSettingsKey.KeyName, cmsSettingsKey.SiteId); // , cmsSettingsKey.KeyGuid);

    public static CmsSettingsKeyKey From(string? keyName, int? siteId) //, Guid keyGuid)
    {
        ArgumentNullException.ThrowIfNull(keyName);

        return new(keyName, siteId); //, keyGuid);
    }
}