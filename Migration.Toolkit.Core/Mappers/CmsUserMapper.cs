using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsUserMapper : EntityMapperBase<KX13.Models.CmsUser, KXO.Models.CmsUser>
{
    private readonly ILogger<CmsUserMapper> _logger;

    public CmsUserMapper(
        ILogger<CmsUserMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
    ) : base(logger, primaryKeyMappingContext, protocol)
    {
        _logger = logger;
    }

    protected override CmsUser? CreateNewInstance(KX13.Models.CmsUser tSourceEntity, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override CmsUser MapInternal(KX13.Models.CmsUser source, CmsUser target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        if (!newInstance && source.UserGuid != target.UserGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            throw new InvalidOperationException("Assertion failed, entity key mismatch.");
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
            if (mappingHelper.TranslateRequiredId<KX13M.CmsRole>(r => r.RoleId, sourceCmsUserRole.RoleId, out var targetRoleId))
            {
                if (target.CmsUserRoles.All(x => x.RoleId != targetRoleId))
                {
                    target.CmsUserRoles.Add(new KXO.Models.CmsUserRole
                    {
                        RoleId = targetRoleId,
                        User = target,
                        ValidTo = sourceCmsUserRole.ValidTo
                    });
                }
            }
        }

        foreach (var sourceCmsUserSite in source.CmsUserSites)
        {
            var userSite = new CmsUserSite();
            if (mappingHelper.TryTranslateId<KX13M.CmsSite>(s => s.SiteId, sourceCmsUserSite.SiteId, out var siteId))
            {
                userSite.SiteId = siteId.Value;
                target.CmsUserSites.Add(userSite);
            }
        }

        return target;

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
        // target.UserMftimestep = source.UserMftimestep;;
    }
}