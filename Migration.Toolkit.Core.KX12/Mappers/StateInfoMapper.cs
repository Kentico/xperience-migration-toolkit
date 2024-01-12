namespace Migration.Toolkit.Core.KX12.Mappers;

using CMS.Globalization;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common.Abstractions;
using Migration.Toolkit.Common.MigrationProtocol;
using Migration.Toolkit.Core.KX12.Contexts;
using Migration.Toolkit.KX12.Models;

public class StateInfoMapper : EntityMapperBase<KX12M.CmsState, StateInfo>
{
    public StateInfoMapper(ILogger<StateInfoMapper> logger, PrimaryKeyMappingContext pkContext, IProtocol protocol) : base(logger, pkContext, protocol)
    {
    }

    protected override StateInfo? CreateNewInstance(CmsState source, MappingHelper mappingHelper, AddFailure addFailure)
        => StateInfo.New();

    protected override StateInfo MapInternal(CmsState source, StateInfo target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        target.StateName = source.StateName;
        target.StateDisplayName = source.StateDisplayName;
        target.StateLastModified = source.StateLastModified;
        target.StateGUID = source.StateGuid;
        target.StateCode = source.StateCode;

        if (mappingHelper.TranslateRequiredId<KX12M.CmsCountry>(k => k.CountryId, source.CountryId, out var countryId))
        {
            target.CountryID = countryId;
        }

        return target;
    }
}