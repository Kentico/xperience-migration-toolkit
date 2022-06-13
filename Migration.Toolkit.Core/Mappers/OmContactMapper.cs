using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class OmContactMapper : IEntityMapper<KX13.Models.OmContact, KXO.Models.OmContact>
{
    private readonly ILogger<OmContactMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IEntityMapper<OmContactStatus, KXO.Models.OmContactStatus> _contactStatusMapper;

    public OmContactMapper(
        ILogger<OmContactMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IEntityMapper<KX13.Models.OmContactStatus, KXO.Models.OmContactStatus> contactStatusMapper
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _contactStatusMapper = contactStatusMapper;
    }

    public IModelMappingResult<KXO.Models.OmContact> Map(KX13.Models.OmContact? source, KXO.Models.OmContact? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.OmContact>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance");
            target = new KXO.Models.OmContact();
            newInstance = true;
        }
        else if (source.ContactGuid != target.ContactGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch");
            return new ModelMappingFailedKeyMismatch<KXO.Models.OmContact>().Log(_logger);
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
        var aggregatedResult = new AggregatedResult<Migration.Toolkit.KXO.Models.OmContact>(target, newInstance);
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
                    aggregatedResult.AddResult(result);
                    return aggregatedResult.Log(_logger);
                }
            }
        }
        else
        {
            target.ContactStatus = null;
        }

        target.ContactSalesForceLeadId = source.ContactSalesForceLeadId;
        target.ContactOwnerUserId = _primaryKeyMappingContext.MapFromSource<K13M.CmsUser>(u => u.UserId, source.ContactOwnerUserId);

        // [ForeignKey("ContactCountryId")]
        // [InverseProperty("OmContacts")]
        // public virtual CmsCountry? ContactCountry { get; set; }
        // [ForeignKey("ContactOwnerUserId")]
        // [InverseProperty("OmContacts")]
        // public virtual CmsUser? ContactOwnerUser { get; set; }
        // [ForeignKey("ContactStateId")]
        // [InverseProperty("OmContacts")]
        // public virtual CmsState? ContactState { get; set; }
        // [ForeignKey("ContactStatusId")]
        // [InverseProperty("OmContacts")]
        // public virtual OmContactStatus? ContactStatus { get; set; }
        // [InverseProperty("ConsentAgreementContact")]
        // public virtual ICollection<CmsConsentAgreement> CmsConsentAgreements { get; set; }
        // [InverseProperty("AccountPrimaryContact")]
        // public virtual ICollection<OmAccount> OmAccountAccountPrimaryContacts { get; set; }
        // [InverseProperty("AccountSecondaryContact")]
        // public virtual ICollection<OmAccount> OmAccountAccountSecondaryContacts { get; set; }
        // [InverseProperty("Contact")]
        // public virtual ICollection<OmAccountContact> OmAccountContacts { get; set; }
        // [InverseProperty("Contact")]
        // public virtual ICollection<OmMembership> OmMemberships { get; set; }
        // [InverseProperty("VisitorToContactContact")]
        // public virtual ICollection<OmVisitorToContact> OmVisitorToContacts { get; set; }

        return new ModelMappingSuccess<KXO.Models.OmContact>(target, newInstance).Log(_logger);
    }
}