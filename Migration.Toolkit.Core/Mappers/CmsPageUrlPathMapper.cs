using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsPageUrlPathMapper : EntityMapperBase<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath>
{
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsPageUrlPathMapper(
        ILogger<CmsPageUrlPathMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
    ) : base(logger, primaryKeyMappingContext, protocol)
    {
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    protected override CmsPageUrlPath? CreateNewInstance(KX13.Models.CmsPageUrlPath source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsPageUrlPath MapInternal(KX13.Models.CmsPageUrlPath source, CmsPageUrlPath target, bool newInstance,
        MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.PageUrlPathGuid != target.PageUrlPathGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsPageUrlPath>().Log(_logger);
        // }

        // target.PageUrlPathId = source.PageUrlPathId;
        target.PageUrlPathGuid = source.PageUrlPathGuid;
        target.PageUrlPathCulture = source.PageUrlPathCulture;
        target.PageUrlPathUrlPath = source.PageUrlPathUrlPath;
        target.PageUrlPathUrlPathHash = source.PageUrlPathUrlPathHash;
        target.PageUrlPathLastModified = source.PageUrlPathLastModified;

        // target.PageUrlPathSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.PageUrlPathSiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.PageUrlPathSiteId, out var siteId))
        {
            target.PageUrlPathSiteId = siteId;
        }

        return target;
    }
}