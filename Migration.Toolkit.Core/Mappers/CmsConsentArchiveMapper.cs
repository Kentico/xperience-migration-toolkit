using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsConsentArchiveMapper : EntityMapperBase<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive>
{
    public CmsConsentArchiveMapper(ILogger<CmsConsentArchiveMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol) : base(logger, primaryKeyMappingContext, protocol)
    {
    }

    protected override CmsConsentArchive? CreateNewInstance(KX13.Models.CmsConsentArchive source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsConsentArchive MapInternal(KX13.Models.CmsConsentArchive source, CmsConsentArchive target, bool newInstance,
        MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.ConsentArchiveGuid!= target.ConsentArchiveGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsentArchive>().Log(_logger);
        // }

        // do not try to insert pk
        // target.ConsentArchiveId = source.ConsentArchiveId;
        target.ConsentArchiveContent = source.ConsentArchiveContent;
        target.ConsentArchiveGuid = source.ConsentArchiveGuid;
        target.ConsentArchiveLastModified = source.ConsentArchiveLastModified;
        target.ConsentArchiveHash = source.ConsentArchiveHash;
        
        //var consentId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentArchiveConsentId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentArchiveConsentId, out var consentId))
        {
            target.ConsentArchiveConsentId = consentId;    
        }

        return target;
    }
}