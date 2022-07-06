using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsConsentMapper : EntityMapperBase<KX13.Models.CmsConsent, KXO.Models.CmsConsent>
{
    public CmsConsentMapper(ILogger<CmsConsentMapper> logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol): base(logger, pkContext, protocol)
    {
    }

    protected override CmsConsent? CreateNewInstance(KX13.Models.CmsConsent source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsConsent MapInternal(KX13.Models.CmsConsent source, CmsConsent target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.ConsentGuid!= target.ConsentGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsent>().Log(_logger);
        // }

        // do not try to insert pk
        // target.ConsentId = source.ConsentId;
        target.ConsentDisplayName = source.ConsentDisplayName;
        target.ConsentName= source.ConsentName;
        target.ConsentContent = source.ConsentContent;
        target.ConsentGuid = source.ConsentGuid;
        target.ConsentLastModified = source.ConsentLastModified;
        target.ConsentHash = source.ConsentHash;
       
        return target;
    }
}