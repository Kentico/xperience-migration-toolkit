using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.MigrateDataProtection;

public class CmsConsentAgreementMapper : IEntityMapper<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement>
{
    private readonly ILogger<CmsConsentAgreementMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsConsentAgreementMapper(ILogger<CmsConsentAgreementMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public ModelMappingResult<KXO.Models.CmsConsentAgreement> Map(KX13.Models.CmsConsentAgreement? source, KXO.Models.CmsConsentAgreement? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsConsentAgreement>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsConsentAgreement();
            newInstance = true;
        }
        else if (source.ConsentAgreementGuid != target.ConsentAgreementGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsentAgreement>();
        }

        // TODO ff: 2022-05-23: Expected OM Contacts sync first
        //var contactId = _primaryKeyMappingContext.MapFromSourceNonRequired<KX13.Models.OmContact>(r => r.ContactId, source.ConsentAgreementContactId);

        //if (!contactId.HasValue)
        //{
        //    _logger.LogTrace("Assertion failed, Contact not found.");
        //    return new ModelMappingFailed<Migration.Toolkit.KXO.Models.CmsConsentAgreement>($"Contact: {source.ConsentAgreementContactId} for entity not found");
        //}

        var consentId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentAgreementConsentId);

        if (!consentId.HasValue)
        {
            _logger.LogTrace("Assertion failed, Consent not found.");
            return new ModelMappingFailed<Migration.Toolkit.KXO.Models.CmsConsentAgreement>($"Consent: {source.ConsentAgreementConsentId} for entity not found");
        }

        // do not try to insert pk
        // target.ConsentAgreementId = source.ConsentAgreementId;
        target.ConsentAgreementGuid = source.ConsentAgreementGuid;
        target.ConsentAgreementRevoked = source.ConsentAgreementRevoked;
        target.ConsentAgreementConsentHash = source.ConsentAgreementConsentHash;
        target.ConsentAgreementTime = source.ConsentAgreementTime;
        // TODO ff: 2022-05-23: Expected OM Contacts sync first
        //target.ConsentAgreementContactId = contactId.Value;
        target.ConsentAgreementContactId = 1;
        target.ConsentAgreementConsentId = consentId.Value;

        return new ModelMappingSuccess<KXO.Models.CmsConsentAgreement>(target, newInstance);
    }
}