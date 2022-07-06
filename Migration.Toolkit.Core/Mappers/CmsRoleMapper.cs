using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsRoleMapper : EntityMapperBase<KX13.Models.CmsRole, KXO.Models.CmsRole>
{
    public CmsRoleMapper(
        ILogger<CmsRoleMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
    ) : base(logger, primaryKeyMappingContext, protocol)
    {
    }

    protected override CmsRole? CreateNewInstance(KX13.Models.CmsRole source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsRole MapInternal(KX13.Models.CmsRole source, CmsRole target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.RoleGuid != target.RoleGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsRole>().Log(_logger);
        // }

        // target.RoleId = source.RoleId;
        target.RoleDisplayName = source.RoleDisplayName;
        target.RoleName = source.RoleName;
        target.RoleDescription = source.RoleDescription;
        // target.SiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.SiteId);
        if (mappingHelper.TranslateId<KX13.Models.CmsSite>(c => c.SiteId, source.SiteId, out var siteId))
        {
            target.SiteId = siteId;
        }

        target.RoleGuid = source.RoleGuid;
        target.RoleLastModified = source.RoleLastModified;
        target.RoleIsDomain = source.RoleIsDomain;

        return target;
    }
}