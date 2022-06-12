using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

//IEntityMapper<KX13.Models.OmContactGroup, KXO.Models.OmContactGroup>

public class OmContactGroupMapper: IEntityMapper<KX13.Models.OmContactGroup, KXO.Models.OmContactGroup>
{
    private readonly ILogger<OmContactGroupMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public OmContactGroupMapper(
        ILogger<OmContactGroupMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext 
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public IModelMappingResult<KXO.Models.OmContactGroup> Map(KX13.Models.OmContactGroup? source, KXO.Models.OmContactGroup? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.OmContactGroup>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new KXO.Models.OmContactGroup();
            newInstance = true;
        }
        else if (source.ContactGroupGuid != target.ContactGroupGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<KXO.Models.OmContactGroup>().Log(_logger);
        }

        // do not try to insert pk
        // target.ContactGroupId = source.ContactGroupId;
        target.ContactGroupName = source.ContactGroupName;
        target.ContactGroupDisplayName = source.ContactGroupDisplayName;
        target.ContactGroupDescription = source.ContactGroupDescription;
        target.ContactGroupDynamicCondition = source.ContactGroupDynamicCondition;
        target.ContactGroupEnabled = source.ContactGroupEnabled;
        target.ContactGroupLastModified = source.ContactGroupLastModified;
        target.ContactGroupGuid = source.ContactGroupGuid;
        target.ContactGroupStatus = source.ContactGroupStatus;

        // TODO tk: 2022-06-13  public virtual ICollection<NewsletterIssueContactGroup> NewsletterIssueContactGroups { get; set; }
        // TODO tk: 2022-06-13  public virtual ICollection<OmContactGroupMember> OmContactGroupMembers { get; set; }

        return new ModelMappingSuccess<KXO.Models.OmContactGroup>(target, newInstance).Log(_logger);
    }
}