using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.MigrateDataProtection;

public class CmsConsentArchiveMapper : IEntityMapper<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive>
{
    private readonly ILogger<CmsConsentArchiveMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsConsentArchiveMapper(ILogger<CmsConsentArchiveMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public ModelMappingResult<KXO.Models.CmsConsentArchive> Map(KX13.Models.CmsConsentArchive? source, KXO.Models.CmsConsentArchive? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsConsentArchive>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsConsentArchive();
            newInstance = true;
        }
        else if (source.ConsentArchiveGuid!= target.ConsentArchiveGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsentArchive>();
        }

        var consentId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsConsent>(r => r.ConsentId, source.ConsentArchiveConsentId);

        if (!consentId.HasValue)
        {
            _logger.LogTrace("Assertion failed, Consent not found.");
            return new ModelMappingFailed<Migration.Toolkit.KXO.Models.CmsConsentArchive>($"Consent: {source.ConsentArchiveConsentId} for entity not found");
        }

        // do not try to insert pk
        // target.ConsentArchiveId = source.ConsentArchiveId;
        target.ConsentArchiveContent = source.ConsentArchiveContent;
        target.ConsentArchiveGuid = source.ConsentArchiveGuid;
        target.ConsentArchiveLastModified = source.ConsentArchiveLastModified;
        target.ConsentArchiveHash = source.ConsentArchiveHash;
        target.ConsentArchiveConsentId = consentId.Value;


        return new ModelMappingSuccess<KXO.Models.CmsConsentArchive>(target, newInstance);
    }
}