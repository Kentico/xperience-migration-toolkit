using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsSiteMapper : EntityMapperBase<KX13.Models.CmsSite, KXO.Models.CmsSite>
{
    public CmsSiteMapper(
        ILogger<CmsSiteMapper> logger,
        PrimaryKeyMappingContext pkContext,
        IMigrationProtocol protocol
    ) : base(logger, pkContext, protocol)
    {
        
    }

    protected override CmsSite? CreateNewInstance(KX13.Models.CmsSite tSourceEntity, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsSite MapInternal(KX13.Models.CmsSite source, CmsSite target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // For site guid match is not required!
        // else if (source.SiteGuid != target.SiteGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsSite>();
        // }

        // target.SiteId = source.SiteId;
        target.SiteName = source.SiteName;
        target.SiteDisplayName = source.SiteDisplayName;
        target.SiteDescription = source.SiteDescription;
        target.SiteStatus = source.SiteStatus;

        var sitePresentationUrl = new Uri(source.SitePresentationUrl); // TODO tk: 2022-07-05 verify
        target.SiteDomainName = sitePresentationUrl.Host;
        // target.SiteDomainName = source.SiteDomainName; // TODO tk: 2022-06-01 check
        target.SiteDefaultVisitorCulture = source.SiteDefaultVisitorCulture;
        // target.SiteGuid = source.SiteGuid; // TODO tk: 2022-05-26 do not rewrite, instead add siteguid to mapping
        target.SiteLastModified = source.SiteLastModified;

        return target;
    }
}