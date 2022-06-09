using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsPageUrlPathMapper : IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath>
{
    private readonly ILogger<CmsPageUrlPathMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsPageUrlPathMapper(
        ILogger<CmsPageUrlPathMapper> logger,
        
        PrimaryKeyMappingContext primaryKeyMappingContext
        )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public IModelMappingResult<KXO.Models.CmsPageUrlPath> Map(KX13.Models.CmsPageUrlPath? source, KXO.Models.CmsPageUrlPath? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsPageUrlPath>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsPageUrlPath();
            newInstance = true;
        }
        else if (source.PageUrlPathGuid != target.PageUrlPathGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsPageUrlPath>().Log(_logger);
        }
        
        // target.PageUrlPathId = source.PageUrlPathId;
        target.PageUrlPathGuid = source.PageUrlPathGuid;
        target.PageUrlPathCulture = source.PageUrlPathCulture;
        target.PageUrlPathUrlPath = source.PageUrlPathUrlPath;
        target.PageUrlPathUrlPathHash = source.PageUrlPathUrlPathHash;
        target.PageUrlPathLastModified = source.PageUrlPathLastModified;

        target.PageUrlPathNodeId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsTree>(c => c.NodeId, source.PageUrlPathNodeId);
        target.PageUrlPathSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.PageUrlPathSiteId);

        return new ModelMappingSuccess<KXO.Models.CmsPageUrlPath>(target, newInstance).Log(_logger);
    }
}