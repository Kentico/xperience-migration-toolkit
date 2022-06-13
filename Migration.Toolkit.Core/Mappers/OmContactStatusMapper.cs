using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class OmContactStatusMapper: IEntityMapper<KX13.Models.OmContactStatus, KXO.Models.OmContactStatus>
{
    private readonly ILogger<OmContactStatusMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public OmContactStatusMapper(
        ILogger<OmContactStatusMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public IModelMappingResult<KXO.Models.OmContactStatus> Map(KX13.Models.OmContactStatus? source, KXO.Models.OmContactStatus? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.OmContactStatus>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance");
            target = new KXO.Models.OmContactStatus();
            newInstance = true;
        }
        else if (source.ContactStatusName != target.ContactStatusName) // TODO tk: 2022-06-13  no guid, no unique value but PK - this might be problem
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch");
            return new ModelMappingFailedKeyMismatch<KXO.Models.OmContactStatus>().Log(_logger);
        }

        // do not try to insert pk
        // target.ContactStatusId = source.ContactStatusId;
        target.ContactStatusName = source.ContactStatusName;
        target.ContactStatusDisplayName = source.ContactStatusDisplayName;
        target.ContactStatusDescription = source.ContactStatusDescription;

        return new ModelMappingSuccess<KXO.Models.OmContactStatus>(target, newInstance).Log(_logger);
    }
}