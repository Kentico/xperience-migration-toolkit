using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateDataProtectionCommandHandler : IRequestHandler<MigrateDataProtectionCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateDataProtectionCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<KX13.Models.CmsConsent, KXO.Models.CmsConsent> _consentMapper;
    private readonly IEntityMapper<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive> _consentArchiveMapper;
    private readonly IEntityMapper<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement> _consentAgreementMapper;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    private static readonly int _batchSize = 1000;

    public MigrateDataProtectionCommandHandler(
        ILogger<MigrateDataProtectionCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.CmsConsent, KXO.Models.CmsConsent> consentMapper,
        IEntityMapper<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive> consentArchiveMapper,
        IEntityMapper<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement> consentAgreementMapper,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _consentMapper = consentMapper;
        _consentArchiveMapper = consentArchiveMapper;
        _consentAgreementMapper = consentAgreementMapper;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = _kxoContextFactory.CreateDbContext();
    }
    
    public async Task<GenericCommandResult> Handle(MigrateDataProtectionCommand request, CancellationToken cancellationToken)
    {
        var batchSize = request.BatchSize;
        
        await MigrateConsent(cancellationToken);
        await MigrateConsentArchive(cancellationToken);
        await MigrateConsentAgreement(cancellationToken, batchSize.HasValue ? batchSize.Value : _batchSize);

        return new GenericCommandResult();
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }

    private async Task<GenericCommandResult> MigrateConsent(CancellationToken cancellationToken)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);
        
        foreach (var kx13Consent in kx13Context.CmsConsents)
        {
            _migrationProtocol.FetchedSource(kx13Consent);
            _logger.LogTrace("Migrating consent {consentName} with ConsentGuid {consentGuid}", kx13Consent.ConsentName, kx13Consent.ConsentGuid);
            
            var kxoConsent = await _kxoContext.CmsConsents.FirstOrDefaultAsync(consent => consent.ConsentGuid == kx13Consent.ConsentGuid, cancellationToken);
            _migrationProtocol.FetchedTarget(kxoConsent);

            var mapped = _consentMapper.Map(kx13Consent, kxoConsent);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case { Success : true } result:
                {
                    var (cmsConsent, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(cmsConsent, nameof(cmsConsent));

                    if (newInstance)
                    {
                        _kxoContext.CmsConsents.Add(cmsConsent);
                    }
                    else
                    {
                        _kxoContext.CmsConsents.Update(cmsConsent);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success<KX13.Models.CmsConsent, KXO.Models.CmsConsent>(kx13Consent, cmsConsent, mapped);
                        _logger.LogInformation(newInstance
                            ? $"CmsConsent: {cmsConsent.ConsentName} with ConsentGuid '{cmsConsent.ConsentGuid}' was inserted."
                            : $"CmsConsent: {cmsConsent.ConsentName} with ConsentGuid '{cmsConsent.ConsentGuid}' was updated.");
                    }
                    catch (DbUpdateException dbUpdateException) when (
                        dbUpdateException.InnerException is SqlException sqlException &&
                        sqlException.Message.Contains("Cannot insert duplicate key row in object")
                    )
                    {
                        await _kxoContext.DisposeAsync();
                        _logger.LogError(
                            "Failed to migrate consent - consent guid: {consentGuid}. Consent needs manual migration. Consent name: {consentName}",
                            kx13Consent.ConsentGuid, kx13Consent.ConsentName);
                        _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                        _migrationProtocol.NeedsManualAction(
                            HandbookReferences.CmsConsentSkip,
                            $"Failed to migrate consent - server guid: {kx13Consent.ConsentGuid}. Use needs manual migration. Consent name: {kx13Consent.ConsentName}",
                            kx13Consent,
                            cmsConsent,
                            mapped
                        );
                        continue;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsConsent>(r => r.ConsentId, kx13Consent.ConsentId, cmsConsent.ConsentId);

                    break;
                }
                default:
                    break;
            }
        }

        return new GenericCommandResult();
    }

    private async Task<GenericCommandResult> MigrateConsentArchive(CancellationToken cancellationToken)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        foreach (var kx13ArchiveConsent in kx13Context.CmsConsentArchives)
        {
            _migrationProtocol.FetchedSource(kx13ArchiveConsent);
            _logger.LogTrace("Migrating consent archive with ConsentArchiveGuid {consentGuid}", kx13ArchiveConsent.ConsentArchiveGuid);

            var kxoConsentArchive = await _kxoContext.CmsConsentArchives.FirstOrDefaultAsync(consentArchive => consentArchive.ConsentArchiveGuid == kx13ArchiveConsent.ConsentArchiveGuid, cancellationToken);
            _migrationProtocol.FetchedTarget(kxoConsentArchive);

            var mapped = _consentArchiveMapper.Map(kx13ArchiveConsent, kxoConsentArchive);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case { Success : true } result:
                {
                    var (cmsConsentArchive, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(cmsConsentArchive, nameof(cmsConsentArchive));

                    if (newInstance)
                    {
                        _kxoContext.CmsConsentArchives.Add(cmsConsentArchive);
                    }
                    else
                    {
                        _kxoContext.CmsConsentArchives.Update(cmsConsentArchive);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success<KX13.Models.CmsConsentArchive, KXO.Models.CmsConsentArchive>(kx13ArchiveConsent, cmsConsentArchive,
                            mapped);
                        _logger.LogInformation(newInstance
                            ? $"CmsConsentArchive with ConsentArchiveGuid '{cmsConsentArchive.ConsentArchiveGuid}' was inserted."
                            : $"CmsConsentArchive with ConsentArchiveGuid '{cmsConsentArchive.ConsentArchiveGuid}' was updated.");
                    }
                    catch (DbUpdateException dbUpdateException) when (
                        dbUpdateException.InnerException is SqlException sqlException &&
                        sqlException.Message.Contains("Cannot insert duplicate key row in object")
                    )
                    {
                        await _kxoContext.DisposeAsync();
                        _logger.LogError(
                            "Failed to migrate consent archive - consent archive guid: {consentArchiveGuid}. Use needs manual migration.",
                            kx13ArchiveConsent.ConsentArchiveGuid);
                        _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                        _migrationProtocol.NeedsManualAction(
                            HandbookReferences.CmsConsentArchiveSkip,
                            $"Failed to migrate consent archive - server guid: {kx13ArchiveConsent.ConsentArchiveGuid}. Use needs manual migration.",
                            kx13ArchiveConsent,
                            cmsConsentArchive,
                            mapped
                        );
                        continue;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.CmsConsentArchive>(r => r.ConsentArchiveGuid,
                        kx13ArchiveConsent.ConsentArchiveId, cmsConsentArchive.ConsentArchiveId);

                    break;
                }
                default:
                    break;
            }
        }

        return new GenericCommandResult();
    }

    private async Task<GenericCommandResult> MigrateConsentAgreement(CancellationToken cancellationToken, int batchSize)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);
        var index = 0;
        var indexFull = 0;
        var consentAgreementUpdates= new List<KXO.Models.CmsConsentAgreement>();
        var consentAgreementNews = new List<KXO.Models.CmsConsentAgreement>();
        var itemsCount = kx13Context.CmsConsentAgreements.Count();

        foreach (var kx13ConsentAgreement in kx13Context.CmsConsentAgreements)
        {
            _migrationProtocol.FetchedSource(kx13ConsentAgreement);
            _logger.LogTrace("Migrating consent agreement with ConsentAgreementGuid {consentAgreementGuid}", kx13ConsentAgreement.ConsentAgreementGuid);

            var kxoConsentAgreement = await _kxoContext.CmsConsentAgreements.FirstOrDefaultAsync(consentAgreement => consentAgreement.ConsentAgreementGuid == kx13ConsentAgreement.ConsentAgreementGuid, cancellationToken);
            _migrationProtocol.FetchedTarget(kxoConsentAgreement);

            var mapped = _consentAgreementMapper.Map(kx13ConsentAgreement, kxoConsentAgreement);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success : true } result)
            {
                var (cmsConsentAgreement, newInstance) = result;
                ArgumentNullException.ThrowIfNull(cmsConsentAgreement, nameof(cmsConsentAgreement));

                if (newInstance)
                {
                    consentAgreementNews.Add(cmsConsentAgreement);
                }
                else
                {
                    consentAgreementUpdates.Add(cmsConsentAgreement);
                }
                //_primaryKeyMappingContext.SetMapping<KX13.Models.CmsConsentAgreement>(r => r.ConsentAgreementGuid, kx13ConsentAgreement.ConsentAgreementId, cmsConsentAgreement.ConsentAgreementId);
            }

            index++;
            indexFull++;

            if (index == batchSize || indexFull == itemsCount)
            {
                _kxoContext.CmsConsentAgreements.AddRange(consentAgreementNews);
                _kxoContext.CmsConsentAgreements.UpdateRange(consentAgreementUpdates);

                try
                {
                    await _kxoContext.SaveChangesAsync(cancellationToken);

                    foreach (var newKx13ConsentAgreement in consentAgreementNews)
                    {
                        _migrationProtocol.Success<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement>(kx13ConsentAgreement, newKx13ConsentAgreement, mapped);
                        _logger.LogInformation($"CmsConsentAgreement: with ConsentAgreementGuid '{newKx13ConsentAgreement.ConsentAgreementGuid}' was inserted.");
                    }

                    foreach (var updateKx13ConsentAgreement in consentAgreementUpdates)
                    {
                        _migrationProtocol.Success<KX13.Models.CmsConsentAgreement, KXO.Models.CmsConsentAgreement>(kx13ConsentAgreement, updateKx13ConsentAgreement, mapped);
                        _logger.LogInformation($"CmsConsentAgreement: with ConsentAgreementGuid '{updateKx13ConsentAgreement.ConsentAgreementGuid}' was updated.");
                    }
                }
                catch (DbUpdateException dbUpdateException) when (
                    dbUpdateException.InnerException is SqlException sqlException &&
                    sqlException.Message.Contains("Cannot insert duplicate key row in object")
                )
                {
                    await _kxoContext.DisposeAsync();

                    foreach (var newKx13ConsentAgreement in consentAgreementNews)
                    {
                        _logger.LogError("Failed to migrate consent agreement - consent guid: {consentAgreementGuid}. Use needs manual migration.}", kx13ConsentAgreement.ConsentAgreementGuid);
                        _migrationProtocol.NeedsManualAction(
                            HandbookReferences.CmsConsentAgreementSkip,
                            $"Failed to migrate consent agreement - server guid: {kx13ConsentAgreement.ConsentAgreementGuid}. Use needs manual migration.",
                            kx13ConsentAgreement,
                            newKx13ConsentAgreement,
                            mapped
                        );
                    }

                    foreach (var updateKx13ConsentAgreement in consentAgreementUpdates)
                    {
                        _logger.LogError("Failed to migrate consent agreement - consent guid: {consentAgreementGuid}. Use needs manual migration.", kx13ConsentAgreement.ConsentAgreementGuid);
                        _migrationProtocol.NeedsManualAction(
                            HandbookReferences.CmsConsentAgreementSkip,
                            $"Failed to migrate consent agreement - server guid: {kx13ConsentAgreement.ConsentAgreementGuid}. Use needs manual migration.",
                            kx13ConsentAgreement,
                            updateKx13ConsentAgreement,
                            mapped
                        );
                    }
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                }
                finally{
                    index = 0;
                    consentAgreementUpdates = new List<KXO.Models.CmsConsentAgreement>();
                    consentAgreementNews = new List<KXO.Models.CmsConsentAgreement>();
                }
            }

            continue;
        }

        return new GenericCommandResult();
    }
}