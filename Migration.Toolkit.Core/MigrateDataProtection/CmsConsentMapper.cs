using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;

namespace Migration.Toolkit.Core.MigrateDataProtection;

public class CmsConsentMapper : IEntityMapper<KX13.Models.CmsConsent, KXO.Models.CmsConsent>
{
    private readonly ILogger<CmsConsentMapper> _logger;

    public CmsConsentMapper(ILogger<CmsConsentMapper> logger)
    {
        _logger = logger;
    }
    
    public ModelMappingResult<KXO.Models.CmsConsent> Map(KX13.Models.CmsConsent? source, KXO.Models.CmsConsent? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsConsent>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsConsent();
            newInstance = true;
        }
        else if (source.ConsentGuid!= target.ConsentGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsConsent>();
        }

        // do not try to insert pk
        // target.ConsentId = source.ConsentId;
        target.ConsentDisplayName = source.ConsentDisplayName;
        target.ConsentName= source.ConsentName;
        target.ConsentContent = source.ConsentContent;
        target.ConsentGuid = source.ConsentGuid;
        target.ConsentLastModified = source.ConsentLastModified;
        target.ConsentHash = source.ConsentHash;
        
       
        return new ModelMappingSuccess<KXO.Models.CmsConsent>(target, newInstance);
    }
}