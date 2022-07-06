using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsConsentAgreementMapper : EntityMapperBase<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement>
{
    public CmsConsentAgreementMapper(ILogger<CmsConsentAgreementMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext, IMigrationProtocol protocol): base(logger, primaryKeyMappingContext, protocol)
    {
    }

    protected override CmsConsentAgreement? CreateNewInstance(KX13.Models.CmsConsentAgreement source, MappingHelper mappingHelper,
        AddFailure addFailure) => new();

    protected override CmsConsentAgreement MapInternal(KX13.Models.CmsConsentAgreement source, CmsConsentAgreement target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.ConsentAgreementGuid != target.ConsentAgreementGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsentAgreement>().Log(_logger);
        // }
        
        // do not try to insert pk
        // target.ConsentAgreementId = source.ConsentAgreementId;
        target.ConsentAgreementGuid = source.ConsentAgreementGuid;
        target.ConsentAgreementRevoked = source.ConsentAgreementRevoked;
        target.ConsentAgreementConsentHash = source.ConsentAgreementConsentHash;
        target.ConsentAgreementTime = source.ConsentAgreementTime;
        
        // target.ConsentAgreementContactId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.OmContact>(c => c.ContactId, source.ConsentAgreementContactId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.OmContact>(c => c.ContactId, source.ConsentAgreementContactId, out var contactId))
        {
            target.ConsentAgreementContactId = contactId;
        }

        // var consentId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentAgreementConsentId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentAgreementConsentId, out var consentId))
        {
            target.ConsentAgreementConsentId = consentId;    
        }

        return target;
    }
}