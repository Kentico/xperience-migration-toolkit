using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services.BulkCopy;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KX13.Models;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

[Obsolete("Currently unneeded leaving for future use is needed", true)]
public class MigrateContactGroupsCommandHandler : IRequestHandler<MigrateContactGroupsCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateContactGroupsCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<OmContactGroup, KXO.Models.OmContactGroup> _contactGroupMapper;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly KxoContext _kxoContext;

    public MigrateContactGroupsCommandHandler(
        ILogger<MigrateContactGroupsCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.OmContactGroup, KXO.Models.OmContactGroup> contactGroupMapper,
        BulkDataCopyService bulkDataCopyService,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
        )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kxoContext = kxoContextFactory.CreateDbContext();
        _kx13ContextFactory = kx13ContextFactory;
        _contactGroupMapper = contactGroupMapper;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
    }
    
    public async Task<GenericCommandResult> Handle(MigrateContactGroupsCommand request, CancellationToken cancellationToken)
    {  
        // var migratedSiteIds = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var kx13OmContactGroups = kx13Context.OmContactGroups
            ;

        foreach (var kx13omContactGroup in kx13OmContactGroups)
        {
            _migrationProtocol.FetchedSource(kx13omContactGroup);
            _logger.LogTrace("Migrating ContactGroup {contactGroup} with ContactGroupGuid {contactGroupGuid}", kx13omContactGroup.ContactGroupName, kx13omContactGroup.ContactGroupGuid);

            var kxoOmContactGroup = await _kxoContext.OmContactGroups
                    .FirstOrDefaultAsync(u => u.ContactGroupGuid == kx13omContactGroup.ContactGroupGuid, cancellationToken)
                ;
            
            _migrationProtocol.FetchedTarget(kxoOmContactGroup);

            var mapped = _contactGroupMapper.Map(kx13omContactGroup, kxoOmContactGroup);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case { Success : true } result:
                {
                    var (omContactGroup, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(omContactGroup, nameof(omContactGroup));

                    if (newInstance)
                    {
                        _kxoContext.OmContactGroups.Add(omContactGroup);
                    }
                    else
                    {
                        _kxoContext.OmContactGroups.Update(omContactGroup);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success(kx13omContactGroup, omContactGroup, mapped);
                        _logger.LogInformation(
                            "OmContactGroup: {ContactGroupName} with ContactGroupGuid '{ContactGroupGuid}' was {Operation}",
                            omContactGroup.ContactGroupName, omContactGroup.ContactGroupGuid, newInstance ? "inserted" : "updated");
                    }
                    catch (Exception ex) // TODO tk: 2022-06-13 handle exceptions
                    {
                        throw;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.OmContactGroup>(r => r.ContactGroupId, kx13omContactGroup.ContactGroupId,
                        omContactGroup.ContactGroupId);

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