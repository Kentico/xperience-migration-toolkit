using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSettingsCategoryMapper : EntityMapperBase<Migration.Toolkit.KX13.Models.CmsSettingsCategory,
    Migration.Toolkit.KXO.Models.CmsSettingsCategory>
{
    private readonly ILogger<CmsSettingsCategoryMapper> _logger;
    private readonly PrimaryKeyMappingContext _pkContext;
    private readonly IEntityMapper<KX13.Models.CmsResource, KXO.Models.CmsResource> _cmsResourceMapper;

    public CmsSettingsCategoryMapper(ILogger<CmsSettingsCategoryMapper> logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol,
        IEntityMapper<KX13M.CmsResource, KXO.Models.CmsResource> cmsResourceMapper) : base(logger, pkContext, protocol)
    {
        _logger = logger;
        _pkContext = pkContext;
        _cmsResourceMapper = cmsResourceMapper;
    }

    protected override KXO.Models.CmsSettingsCategory? CreateNewInstance(CmsSettingsCategory source, MappingHelper mappingHelper,
        AddFailure addFailure) => new();
    

    protected override KXOM.CmsSettingsCategory MapInternal(KX13.Models.CmsSettingsCategory source, KXOM.CmsSettingsCategory target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // no category guid to match on...
        // else if (source.CategoryName != target.CategoryName)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSettingsCategory>();
        // }

        // map entity
        // target.CategoryId = source.CategoryId;

        if (newInstance)
        {
            target.CategoryOrder = source.CategoryOrder;
            target.CategoryName = source.CategoryName;
            target.CategoryDisplayName = source.CategoryDisplayName;
            target.CategoryIdpath = source.CategoryIdpath;
            target.CategoryLevel = source.CategoryLevel;
            target.CategoryChildCount = source.CategoryChildCount;
            target.CategoryIconPath = source.CategoryIconPath;
            target.CategoryIsGroup = source.CategoryIsGroup;
            target.CategoryIsCustom = source.CategoryIsCustom;
        }

        if (source.CategoryResource != null)
        {
            if (target.CategoryResource != null)
            {
                // skip if target is present
                _logger.LogTrace("Skipping category resource '{resourceGuid}', already present in target instance.", target.CategoryResource.ResourceGuid);
                _pkContext.SetMapping<KX13.Models.CmsResource>(r => r.ResourceId, source.CategoryResourceId.Value, target.CategoryResourceId.Value);
            }
            else
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
                        addFailure(new MapperResultFailure<KXOM.CmsSettingsCategory>(result.HandbookReference));
                        break;
                    }
                }
            }
        }
        else if(mappingHelper.TranslateId<KX13.Models.CmsResource>(r => r.ResourceId, source.CategoryResourceId, out var categoryResourceId))
        {
            target.CategoryResourceId = categoryResourceId;
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
                    addFailure(new MapperResultFailure<KXOM.CmsSettingsCategory>(result.HandbookReference));
                    break;
                }
            }
        }
        else if(mappingHelper.TranslateId<KX13M.CmsCategory>(c => c.CategoryId, source.CategoryParentId, out var categoryParentId))
        {
            target.CategoryParentId = categoryParentId;
        }

        return target;
    }
}