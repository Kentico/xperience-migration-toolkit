using System.Security.Cryptography.Xml;
using AngleSharp.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Helpers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services.BulkCopy;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateContactManagementCommandHandler : IRequestHandler<MigrateContactManagementCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateContactManagementCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<K13M.OmContact, KXOM.OmContact> _contactMapper;
    private readonly BulkDataCopyService _bulkDataCopyService;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly KxoContext _kxoContext;

    public MigrateContactManagementCommandHandler(
        ILogger<MigrateContactManagementCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.OmContact, KXO.Models.OmContact> contactMapper,
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
        _contactMapper = contactMapper;
        _bulkDataCopyService = bulkDataCopyService;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
    }
    
    public async Task<GenericCommandResult> Handle(MigrateContactManagementCommand request, CancellationToken cancellationToken)
    {  
        // var explicitSiteIdMapping = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();

        // TODO tk: 2022-06-13 check field length
        // _bulkDataCopyService.CheckIfDataExistsInTargetTable("OM_Contact");
        
        var bulkCopyRequest = new BulkCopyRequest("OM_Contact",
            s => s != "ContactID",
            reader => true, 150000,
            new List<string>
            {
                "ContactID", "ContactFirstName", "ContactMiddleName", "ContactLastName", "ContactJobTitle", "ContactAddress1", "ContactCity",
                "ContactZIP", "ContactStateID", "ContactCountryID", "ContactMobilePhone", "ContactBusinessPhone", "ContactEmail", "ContactBirthday",
                "ContactGender", "ContactStatusID", "ContactNotes", "ContactOwnerUserID", "ContactMonitored", "ContactGUID", "ContactLastModified",
                "ContactCreated", "ContactBounces", "ContactCampaign", "ContactSalesForceLeadID", "ContactSalesForceLeadReplicationDisabled",
                "ContactSalesForceLeadReplicationDateTime", "ContactSalesForceLeadReplicationSuspensionDateTime", "ContactCompanyName",
                "ContactSalesForceLeadReplicationRequired"
            },
            (ordinal, columnName, value) => 
            {
                if (columnName == "ContactCompanyName")
                {
                    return SqlDataTypeHelper.TruncateString(value, 100); // TODO tk: 2022-06-13 log truncation
                }

                return value;
            }  
        );
        // TODO tk: 2022-06-13 also migrate status with contact
        _logger.LogTrace("Bulk data copy request: {request}", bulkCopyRequest);
        _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
        return new GenericCommandResult();
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var contactsCount = kx13Context.OmContacts.Count();
        _logger.LogInformation("Total OmContact count {count}", contactsCount);

        var chunkSize = 5000;
        for (var chunkIndex = 0; chunkIndex < contactsCount + chunkSize; chunkIndex += chunkSize)
        {
            var contactChunk = await kx13Context.OmContacts
                .Include(c => c.ContactStatus)
                .AsSplitQuery().Skip(chunkIndex).Take(chunkSize).ToListAsync(cancellationToken: cancellationToken);

            var contactChunkGuids = contactChunk.Select(x => x.ContactGuid).ToList();

            var targetContacts = await _kxoContext.OmContacts
                .Include(c => c.ContactStatus)
                .Where(u => contactChunkGuids.Contains(u.ContactGuid))
                .ToDictionaryAsync(x => x.ContactGuid, cancellationToken: cancellationToken);

            var savedContacts = new List<(K13M.OmContact, KXOM.OmContact)>();
            foreach (var kx13OmContact in contactChunk)
            {
                _migrationProtocol.FetchedSource(kx13OmContact);
                _logger.LogTrace("Migrating Contact with ContactGroupGuid {contactGuid}", kx13OmContact.ContactGuid);

                var kxoOmContact = targetContacts.TryGetValue(kx13OmContact.ContactGuid, out var target) ? target : null;

                _migrationProtocol.FetchedTarget(kxoOmContact);

                var mapped = _contactMapper.Map(kx13OmContact, kxoOmContact);
                _migrationProtocol.MappedTarget(mapped);

                switch (mapped)
                {
                    case ModelMappingSuccess<KXOM.OmContact>(var omContact, var newInstance):
                        ArgumentNullException.ThrowIfNull(omContact, nameof(omContact));

                        if (newInstance)
                        {
                            _kxoContext.OmContacts.Add(omContact);
                        }
                        else
                        {
                            _kxoContext.OmContacts.Update(omContact);
                        }
                        savedContacts.Add((kx13OmContact, omContact));

                        try
                        {
                            _migrationProtocol.Success(kx13OmContact, omContact, mapped);
                            _logger.LogInformation(
                                "OmContact: with ContactGuid '{contactGuid}' was {operation}.", omContact.ContactGuid,
                                newInstance ? "inserted" : "updated");
                        }
                        catch (Exception ex) // TODO tk: 2022-06-13 handle exceptions
                        {
                            throw;
                        }

                        break;
                    default:
                        break;
                }
            }
            
            await _kxoContext.SaveChangesAsync(cancellationToken);
            
            foreach (var (kx13OmContact, omContact) in savedContacts)
            {
                _primaryKeyMappingContext.SetMapping<KX13.Models.OmContact>(r => r.ContactId, kx13OmContact.ContactId, omContact.ContactId);    
            }
            
            _logger.LogInformation("OmContact chunk of size {size} completed", contactChunkGuids.Count);
        }

        return new GenericCommandResult();
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}