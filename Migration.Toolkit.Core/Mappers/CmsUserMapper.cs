using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsUserMapper: IEntityMapper<KX13.Models.CmsUser, KXO.Models.CmsUser>
{
    private readonly ILogger<CmsUserMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsUserMapper(
        ILogger<CmsUserMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext 
    )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public IModelMappingResult<KXO.Models.CmsUser> Map(KX13.Models.CmsUser? source, KXO.Models.CmsUser? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.CmsUser>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new KXO.Models.CmsUser();
            newInstance = true;
        }
        else if (source.UserGuid != target.UserGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<KXO.Models.CmsUser>().Log(_logger);
        }

        // do not try to insert pk
        // target.UserId = source.UserId;
        target.UserName = source.UserName;
        target.FirstName = source.FirstName;
        target.LastName = source.LastName;
        target.Email = source.Email;
        target.UserPassword = source.UserPassword;
        target.UserEnabled = source.UserEnabled;
        target.UserCreated = source.UserCreated;
        target.LastLogon = source.LastLogon;
        target.UserGuid = source.UserGuid;
        target.UserLastModified = source.UserLastModified;
        target.UserSecurityStamp = source.UserSecurityStamp;

        // TODO tk: 2022-05-18 deduced - check
        target.UserAdministrationAccess = source.UserPrivilegeLevel == 3;
        // TODO tk: 2022-05-18 deduce info
        target.UserIsPendingRegistration = false;
        // TODO tk: 2022-05-18 deduce info
        target.UserPasswordLastChanged = null;
        // TODO tk: 2022-05-18 deduce info
        target.UserRegistrationLinkExpiration = DateTime.Now.AddDays(365);
        
        foreach (var sourceCmsUserRole in source.CmsUserRoles)
        {
            var targetRoleId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsRole>(r => r.RoleId, sourceCmsUserRole.RoleId);
            if (target.CmsUserRoles.All(x => x.RoleId != targetRoleId))
            {
                target.CmsUserRoles.Add(new KXO.Models.CmsUserRole
                {
                    RoleId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsRole>(r => r.RoleId, sourceCmsUserRole.RoleId),
                    User = target,
                    ValidTo = sourceCmsUserRole.ValidTo
                });    
            }
        }

        return new ModelMappingSuccess<KXO.Models.CmsUser>(target, newInstance).Log(_logger);

        // removed in kxo
        // target.MiddleName = source.MiddleName;
        // target.FullName = source.FullName;
        // target.PreferredCultureCode = source.PreferredCultureCode;
        // target.PreferredUicultureCode = source.PreferredUicultureCode;
        // target.UserIsExternal = source.UserIsExternal;
        // target.UserPasswordFormat = source.UserPasswordFormat;
        // target.UserStartingAliasPath = source.UserStartingAliasPath;
        // target.UserLastLogonInfo = source.UserLastLogonInfo;
        // target.UserIsHidden = source.UserIsHidden;
        // target.UserIsDomain = source.UserIsDomain;
        // target.UserHasAllowedCultures = source.UserHasAllowedCultures;
        // target.UserMfrequired = source.UserMfrequired;
        // target.UserPrivilegeLevel = source.UserPrivilegeLevel;
        // target.UserMftimestep = source.UserMftimestep;
    }
}