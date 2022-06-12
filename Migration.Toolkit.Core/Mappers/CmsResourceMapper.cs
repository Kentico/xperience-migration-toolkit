using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core.Mappers;

public class CmsResourceMapper : IEntityMapper<Migration.Toolkit.KX13.Models.CmsResource, Migration.Toolkit.KXO.Models.CmsResource>
{
    private readonly ILogger<CmsResourceMapper> _logger;

    public CmsResourceMapper(ILogger<CmsResourceMapper> logger)
    {
        _logger = logger;
    }

    public IModelMappingResult<Migration.Toolkit.KXO.Models.CmsResource> Map(Migration.Toolkit.KX13.Models.CmsResource? source,
        Migration.Toolkit.KXO.Models.CmsResource? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsResource>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsResource();
            newInstance = true;
        }
        else if (source.ResourceGuid != target.ResourceGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch on resources S={sourceGuild}, T={targetGuid}", source.ResourceGuid, target.ResourceGuid);
            // allowing to run through, same resource is not required for target instance
            // return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsResource>();
        }

        // avoid updating resource
        if(!newInstance) return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsResource>(target, newInstance).Log(_logger);
        
        // map entity
        // target.ResourceId = source.ResourceId;
        target.ResourceDisplayName = source.ResourceDisplayName;
        target.ResourceName = source.ResourceName;
        target.ResourceDescription = source.ResourceDescription;
        target.ShowInDevelopment = source.ShowInDevelopment;
        target.ResourceUrl = source.ResourceUrl;
        target.ResourceGuid = source.ResourceGuid;
        target.ResourceLastModified = source.ResourceLastModified;
        target.ResourceIsInDevelopment = source.ResourceIsInDevelopment;
        target.ResourceHasFiles = source.ResourceHasFiles;
        target.ResourceVersion = source.ResourceVersion;
        target.ResourceAuthor = source.ResourceAuthor;
        target.ResourceInstallationState = source.ResourceInstallationState;
        target.ResourceInstalledVersion = source.ResourceInstalledVersion;

        return new ModelMappingSuccess<Migration.Toolkit.KXO.Models.CmsResource>(target, newInstance).Log(_logger);
    }
}