using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsRoleMapper : IEntityMapper<KX13.Models.CmsRole, KXO.Models.CmsRole>
{
    private readonly ILogger<CmsRoleMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsRoleMapper(
        ILogger<CmsRoleMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }

    public IModelMappingResult<KXO.Models.CmsRole> Map(KX13.Models.CmsRole? source, KXO.Models.CmsRole? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsRole>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new Migration.Toolkit.KXO.Models.CmsRole();
            newInstance = true;
        }
        else if (source.RoleGuid != target.RoleGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsRole>();
        }
        
        // target.RoleId = source.RoleId;
        target.RoleDisplayName = source.RoleDisplayName;
        target.RoleName = source.RoleName;
        target.RoleDescription = source.RoleDescription;
        target.SiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.SiteId);
        target.RoleGuid = source.RoleGuid;
        target.RoleLastModified = source.RoleLastModified;
        target.RoleIsDomain = source.RoleIsDomain;

        // [ForeignKey("SiteId")]
        // [InverseProperty("CmsRoles")]
        // public virtual CmsSite? Site { get; set; }
        // [InverseProperty("Role")]
        // public virtual ICollection<CmsAclitem> CmsAclitems { get; set; }
        // [InverseProperty("Role")]
        // public virtual ICollection<CmsUserRole> CmsUserRoles { get; set; }
        // [InverseProperty("Role")]
        // public virtual ICollection<CmsWidgetRole> CmsWidgetRoles { get; set; }
        // [InverseProperty("Role")]
        // public virtual ICollection<CmsWorkflowStepRole> CmsWorkflowStepRoles { get; set; }
        // [InverseProperty("Role")]
        // public virtual ICollection<MediaLibraryRolePermission> MediaLibraryRolePermissions { get; set; }
        //
        // [ForeignKey("RoleId")]
        // [InverseProperty("Roles")]
        // public virtual ICollection<CmsForm> Forms { get; set; }
        // [ForeignKey("RoleId")]
        // [InverseProperty("Roles")]
        // public virtual ICollection<CmsMembership> Memberships { get; set; }
        // [ForeignKey("RoleId")]
        // [InverseProperty("Roles")]
        // public virtual ICollection<CmsPermission> Permissions { get; set; }
        
        return new ModelMappingSuccess<KXO.Models.CmsRole>(target, newInstance);
    }
}