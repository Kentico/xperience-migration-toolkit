using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;


namespace Migration.Toolkit.Core.Mappers;

public class OmContactMapper : EntityMapperBase<KX13.Models.OmContact, KXO.Models.OmContact>
{
    private readonly ILogger<OmContactMapper> _logger;
    private readonly IEntityMapper<KX13M.OmContactStatus, KXO.Models.OmContactStatus> _contactStatusMapper;

    public OmContactMapper(
        ILogger<OmContactMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IEntityMapper<KX13.Models.OmContactStatus, KXO.Models.OmContactStatus> contactStatusMapper,
        IMigrationProtocol protocol
    ): base(logger, primaryKeyMappingContext, protocol)
    {
        _logger = logger;
        _contactStatusMapper = contactStatusMapper;
    }

    protected override OmContact? CreateNewInstance(KX13.Models.OmContact tSourceEntity, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override KXOM.OmContact MapInternal(KX13.Models.OmContact source, KXOM.OmContact target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        if (!newInstance && source.ContactGuid != target.ContactGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch");
            throw new InvalidOperationException("Assertion failed, entity key mismatch");
        }

        // do not try to insert pk
        // target.ContactId = source.ContactId;
        target.ContactFirstName = source.ContactFirstName;
        target.ContactMiddleName = source.ContactMiddleName;
        target.ContactLastName = source.ContactLastName;
        target.ContactJobTitle = source.ContactJobTitle;
        target.ContactAddress1 = source.ContactAddress1;
        target.ContactCity = source.ContactCity;
        target.ContactZip = source.ContactZip;
        target.ContactMobilePhone = source.ContactMobilePhone;
        target.ContactBusinessPhone = source.ContactBusinessPhone;
        target.ContactEmail = source.ContactEmail;
        target.ContactBirthday = source.ContactBirthday;
        target.ContactGender = source.ContactGender;
        target.ContactNotes = source.ContactNotes;
        target.ContactMonitored = source.ContactMonitored;
        target.ContactGuid = source.ContactGuid;
        target.ContactLastModified = source.ContactLastModified;
        target.ContactCreated = source.ContactCreated;
        target.ContactBounces = source.ContactBounces;
        target.ContactCampaign = source.ContactCampaign;
        target.ContactSalesForceLeadReplicationDisabled = source.ContactSalesForceLeadReplicationDisabled;
        target.ContactSalesForceLeadReplicationDateTime = source.ContactSalesForceLeadReplicationDateTime;
        target.ContactSalesForceLeadReplicationSuspensionDateTime = source.ContactSalesForceLeadReplicationSuspensionDateTime;
        target.ContactCompanyName = source.ContactCompanyName;
        target.ContactSalesForceLeadReplicationRequired = source.ContactSalesForceLeadReplicationRequired;

        // TODO tk: 2022-06-13 resolve migration of target.ContactStateId = _primaryKeyMappingContext.MapFromSource<K13M.CmsState>(u => u.StateId, source.ContactStateId);
        // TODO tk: 2022-06-13 resolve migration of target.ContactCountryId = _primaryKeyMappingContext.MapFromSource<K13M.CmsCountry>(u => u.CountryId, source.ContactCountryId);


        // target.ContactStatusId = _primaryKeyMappingContext.MapFromSource<K13M.OmContactStatus>(u => u.ContactStatusId, source.ContactStatusId);
        if (source.ContactStatus != null)
        {
            switch (_contactStatusMapper.Map(source.ContactStatus, target.ContactStatus))
            {
                case { Success: true } result:
                {
                    target.ContactStatus = result.Item;
                    break;
                }
                case { Success: false } result:
                {
                    addFailure(new MapperResultFailure<OmContact>(result?.HandbookReference));
                    break;
                }
            }
        }
        else
        {
            target.ContactStatus = null;
        }

        target.ContactSalesForceLeadId = source.ContactSalesForceLeadId;
        // target.ContactOwnerUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(u => u.UserId, source.ContactOwnerUserId);
        if (mappingHelper.TranslateId<KX13M.CmsUser>(u => u.UserId, source.ContactOwnerUserId, out var userId))
        {
            target.ContactOwnerUserId = userId;
        }

        return target;
    }
}