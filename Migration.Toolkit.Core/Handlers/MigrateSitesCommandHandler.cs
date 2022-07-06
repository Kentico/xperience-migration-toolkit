using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

// ReSharper disable once UnusedType.Global
public class MigrateSitesCommandHandler: IRequestHandler<MigrateSitesCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateSitesCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<KX13.Models.CmsSite, KXO.Models.CmsSite> _cmsSiteMapper;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    public MigrateSitesCommandHandler(
        ILogger<MigrateSitesCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.CmsSite, KXO.Models.CmsSite> cmsSiteMapper,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _cmsSiteMapper = cmsSiteMapper;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = _kxoContextFactory.CreateDbContext();
    }
    
    public async Task<GenericCommandResult> Handle(MigrateSitesCommand request, CancellationToken cancellationToken)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        foreach (var kx13CmsSite in kx13Context.CmsSites)
        {
            _migrationProtocol.FetchedSource(kx13CmsSite);
            _logger.LogTrace("Migrating site {siteName} with UserGuid {siteGuid}", kx13CmsSite.SiteName, kx13CmsSite.SiteGuid);

            var targetSiteId = _primaryKeyMappingContext.MapFromSourceOrNull<KX13.Models.CmsSite>(s => s.SiteId, kx13CmsSite.SiteId);
            if (targetSiteId is null)
            {
                // TODO tk: 2022-05-26 add site guid mapping
                _logger.LogWarning("Site '{siteName}' with Guid {guid} migration skipped", kx13CmsSite.SiteName, kx13CmsSite.SiteGuid);
                continue;
            }
            
            var kxoCmsSite = await _kxoContext.CmsSites.FirstOrDefaultAsync(u => u.SiteId == targetSiteId, cancellationToken);
            _migrationProtocol.FetchedTarget(kxoCmsSite);

            var mapped = _cmsSiteMapper.Map(kx13CmsSite, kxoCmsSite);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case { Success : true } result:
                {
                    var (cmsSite, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(cmsSite, nameof(cmsSite));

                    if (newInstance)
                    {
                        _kxoContext.CmsSites.Add(cmsSite);
                    }
                    else
                    {
                        _kxoContext.CmsSites.Update(cmsSite);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success(kx13CmsSite, cmsSite, mapped);
                        _logger.LogInformation(newInstance
                            ? $"CmsSite: {cmsSite.SiteName} with SiteGuid '{cmsSite.SiteGuid}' was inserted."
                            : $"CmsSite: {cmsSite.SiteName} with SiteGuid '{cmsSite.SiteGuid}' was updated.");
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsSite>(r => r.SiteId, kx13CmsSite.SiteId, cmsSite.SiteId);

                    break;
                }
                default:
                    break;
            }
        }

        return new GenericCommandResult();
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}