using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class OmContactStatusMapper : EntityMapperBase<KX13.Models.OmContactStatus, KXOM.OmContactStatus>
{
    public OmContactStatusMapper(
        ILogger<OmContactStatusMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    ) : base(logger, primaryKeyMappingContext, migrationProtocol)
    {
    }

    protected override KXO.Models.OmContactStatus? CreateNewInstance(OmContactStatus tSourceEntity, MappingHelper mappingHelper,
        AddFailure addFailure) => new();

    protected override KXOM.OmContactStatus MapInternal(KX13.Models.OmContactStatus source, KXOM.OmContactStatus target, bool newInstance,
        MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (!newInstance && source.ContactStatusName != target.ContactStatusName) // TODO tk: 2022-06-13  no guid, no unique value but PK - this might be problem
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch");
        //     return new ModelMappingFailedKeyMismatch<KXO.Models.OmContactStatus>().Log(_logger);
        // }

        // do not try to insert pk
        // target.ContactStatusId = source.ContactStatusId;
        target.ContactStatusName = source.ContactStatusName;
        target.ContactStatusDisplayName = source.ContactStatusDisplayName;
        target.ContactStatusDescription = source.ContactStatusDescription;

        return target;
    }
}