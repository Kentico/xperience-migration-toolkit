using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateUsersCommandHandler: IRequestHandler<MigrateUsersCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateUsersCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<KX13.Models.CmsUser, KXO.Models.CmsUser> _userMapper;
    private readonly IEntityMapper<KX13.Models.CmsRole, KXO.Models.CmsRole> _roleMapper;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    public MigrateUsersCommandHandler(
        ILogger<MigrateUsersCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.CmsUser, KXO.Models.CmsUser> userMapper,
        IEntityMapper<KX13.Models.CmsRole, KXO.Models.CmsRole> roleMapper,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _userMapper = userMapper;
        _roleMapper = roleMapper;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = _kxoContextFactory.CreateDbContext();
    }
    
    public async Task<GenericCommandResult> Handle(MigrateUsersCommand request, CancellationToken cancellationToken)
    {
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        await RequireMigratedCmsRoles(kx13Context, cancellationToken, migratedSiteIds);

        var kx13CmsUsers = kx13Context.CmsUsers
                .Include(u => u.CmsUserRoles.Where(x => migratedSiteIds.Contains(x.Role.SiteId) || x.Role.SiteId == null))
                .ThenInclude(ur => ur.Role)
            ;

        foreach (var kx13User in kx13CmsUsers)
        {
            _migrationProtocol.FetchedSource(kx13User);
            _logger.LogTrace("Migrating user {UserName} with UserGuid {UserGuid}", kx13User.UserName, kx13User.UserGuid);

            var kxoUser = await _kxoContext.CmsUsers
                    .Include(u => u.CmsUserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.UserGuid == kx13User.UserGuid, cancellationToken)
                ;
            
            _migrationProtocol.FetchedTarget(kxoUser);

            if (kx13User.UserPrivilegeLevel == 3 && kxoUser != null)
            {
                _migrationProtocol.Warning(HandbookReferences.CmsUserAdminUserSkip, kx13User, kxoUser);
                _logger.LogInformation("User with guid {Guid} is administrator, you need to update administrators manually => skipping", kx13User.UserGuid);
                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsUser>(r => r.UserId, kx13User.UserId, kxoUser.UserId);
                continue;
            }
            
            if (kxoUser?.UserName == "public" || kx13User.UserName == "public")
            {
                _migrationProtocol.Warning(HandbookReferences.CmsUserPublicUserSkip, kx13User, kxoUser);
                _logger.LogInformation("User with guid {Guid} is public user, special case that can't be migrated => skipping", kxoUser?.UserGuid ?? kx13User.UserGuid);
                if (kxoUser != null)
                {
                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsUser>(r => r.UserId, kx13User.UserId, kxoUser.UserId);    
                }
                continue;
            }
            
            var mapped = _userMapper.Map(kx13User, kxoUser);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success : true } result)
            {
                var (cmsUser, newInstance) = result;

                if (newInstance)
                {
                    _kxoContext.CmsUsers.Add(cmsUser);
                }
                else
                {
                    _kxoContext.CmsUsers.Update(cmsUser);
                }

                try
                {
                    await _kxoContext.SaveChangesAsync(cancellationToken);

                    _migrationProtocol.Success(kx13User, cmsUser, mapped);
                    _logger.LogInformation(newInstance
                        ? $"CmsUser: {cmsUser.UserName} with UserGuid '{cmsUser.UserGuid}' was inserted."
                        : $"CmsUser: {cmsUser.UserName} with UserGuid '{cmsUser.UserGuid}' was updated.");
                }
                catch (DbUpdateException dbUpdateException) when (
                    dbUpdateException.InnerException is SqlException sqlException &&
                    sqlException.Message.Contains("Cannot insert duplicate key row in object") &&
                    sqlException.Message.Contains("IX_CMS_User_UserName") &&
                    sqlException.Message.Contains("CMS_User")
                )
                {
                    await _kxoContext.DisposeAsync();
                    // TODO tk: 2022-05-18 protocol - request manual migration
                    _logger.LogError(
                        "Failed to migrate user, possibly due to duplicated userName - user guid: {UserGuid}. Use needs manual migration. Email: {Email} - it is recommended to fix source data before proceeding",
                        kx13User.UserGuid, kx13User.Email);
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                    _migrationProtocol.NeedsManualAction(
                        HandbookReferences.CmsUserUserNameConstraintBroken,
                        $"Failed to migrate user, possibly due to duplicated userName - user guid: {kx13User.UserGuid}. Use needs manual migration. Email: {kx13User.Email}",
                        kx13User,
                        cmsUser,
                        mapped
                    );
                    continue;
                }
                catch (DbUpdateException dbUpdateException) when (
                    dbUpdateException.InnerException is SqlException sqlException &&
                    sqlException.Message.Contains("Cannot insert duplicate key row in object") &&
                    sqlException.Message.Contains("IX_CMS_User_Email") &&
                    sqlException.Message.Contains("CMS_User")
                )
                {
                    await _kxoContext.DisposeAsync();
                    _logger.LogError(
                        "Failed to migrate user, possibly due to duplicated email - user guid: {UserGuid}. Use needs manual migration. Email: {Email}",
                        kx13User.UserGuid, kx13User.Email);
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                    _migrationProtocol.NeedsManualAction(
                        HandbookReferences.CmsUserEmailConstraintBroken,
                        $"Failed to migrate user, possibly due to duplicated email - user guid: {kx13User.UserGuid}. Use needs manual migration. Email: {kx13User.Email}",
                        kx13User,
                        cmsUser,
                        mapped
                    );
                    continue;
                }

                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsUser>(r => r.UserId, kx13User.UserId, cmsUser.UserId);
            }
        }

        return new GenericCommandResult();
    }

    private async Task RequireMigratedCmsRoles(KX13Context kx13Context, CancellationToken cancellationToken, List<int?> migratedSiteIds)
    {
        var kx13CmsRoles = kx13Context.CmsRoles
            .Where(x => migratedSiteIds.Contains(x.SiteId) || x.SiteId == null);

        foreach (var kx13CmsRole in kx13CmsRoles)
        {
            _migrationProtocol.FetchedSource(kx13CmsRole);

            var kxoCmsRole = await _kxoContext.CmsRoles
                .FirstOrDefaultAsync(x => x.RoleGuid == kx13CmsRole.RoleGuid, cancellationToken: cancellationToken);

            _migrationProtocol.FetchedTarget(kxoCmsRole);

            var mapped = _roleMapper.Map(kx13CmsRole, kxoCmsRole);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success : true } result)
            {
                var (cmsRole, newInstance) = result;
                ArgumentNullException.ThrowIfNull(cmsRole, nameof(cmsRole));

                if (newInstance)
                {
                    _kxoContext.CmsRoles.Add(cmsRole);
                }
                else
                {
                    _kxoContext.CmsRoles.Update(cmsRole);
                }

                try
                {
                    await _kxoContext.SaveChangesAsync(cancellationToken);

                    _migrationProtocol.Success(kx13CmsRole, cmsRole, mapped);
                    _logger.LogInformation(newInstance
                        ? $"CmsRole: {cmsRole.RoleGuid} was inserted."
                        : $"CmsRole: {cmsRole.RoleGuid} was updated.");
                }
                catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
                {
                    throw;
                }

                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsRole>(
                    r => r.RoleId,
                    kx13CmsRole.RoleId,
                    cmsRole.RoleId
                );
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}