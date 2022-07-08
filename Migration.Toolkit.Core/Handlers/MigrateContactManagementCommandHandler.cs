using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Helpers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services.BulkCopy;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateContactManagementCommandHandler : IRequestHandler<MigrateContactManagementCommand, CommandResult>, IDisposable
{
    private readonly ILogger<MigrateContactManagementCommandHandler> _logger;
    private readonly BulkDataCopyService _bulkDataCopyService;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly KXO.Context.KxoContext _kxoContext;

    public MigrateContactManagementCommandHandler(
        ILogger<MigrateContactManagementCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        BulkDataCopyService bulkDataCopyService,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContext = kxoContextFactory.CreateDbContext();
        _bulkDataCopyService = bulkDataCopyService;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
    }

    public Task<CommandResult> Handle(MigrateContactManagementCommand request, CancellationToken cancellationToken)
    {
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<CmsSite>(s => s.SiteId).Keys.ToList();
        if (MigrateContacts() is { } ccr) return Task.FromResult(ccr);
        if (MigrateContactActivities(migratedSiteIds) is { } acr) return Task.FromResult(acr);

        return Task.FromResult<CommandResult>(new GenericCommandResult());
    }

    #region "Migrate contacts"

    private CommandResult? MigrateContacts()
    {
        var requiredColumnsForContactMigration = new Dictionary<string, string>
        {
            { nameof(OmContact.ContactId), nameof(KXO.Models.OmContact.ContactId) },
            { nameof(OmContact.ContactFirstName), nameof(KXO.Models.OmContact.ContactFirstName) },
            { nameof(OmContact.ContactMiddleName), nameof(KXO.Models.OmContact.ContactMiddleName) },
            { nameof(OmContact.ContactLastName), nameof(KXO.Models.OmContact.ContactLastName) },
            { nameof(OmContact.ContactJobTitle), nameof(KXO.Models.OmContact.ContactJobTitle) },
            { nameof(OmContact.ContactAddress1), nameof(KXO.Models.OmContact.ContactAddress1) },
            { nameof(OmContact.ContactCity), nameof(KXO.Models.OmContact.ContactCity) },
            { nameof(OmContact.ContactZip), nameof(KXO.Models.OmContact.ContactZip) },
            { nameof(OmContact.ContactStateId), nameof(KXO.Models.OmContact.ContactStateId) }, // No support 2022-07-07 but needs to be mapped because of constraint
            { nameof(OmContact.ContactCountryId), nameof(KXO.Models.OmContact.ContactCountryId) }, // No support 2022-07-07 but needs to be mapped because of constraint
            { nameof(OmContact.ContactMobilePhone), nameof(KXO.Models.OmContact.ContactMobilePhone) },
            { nameof(OmContact.ContactBusinessPhone), nameof(KXO.Models.OmContact.ContactBusinessPhone) },
            { nameof(OmContact.ContactEmail), nameof(KXO.Models.OmContact.ContactEmail) },
            // No support 2022-07-07  { nameof(OmContact.ContactBirthday), nameof(KXO.Models.OmContact.ContactBirthday) },
            { nameof(OmContact.ContactGender), nameof(KXO.Models.OmContact.ContactGender) },
            // { nameof(OmContact.ContactStatusId), nameof(KXO.Models.OmContact.ContactStatusId) }, // No support 2022-07-07  but needs to be mapped because of constraint
            { nameof(OmContact.ContactNotes), nameof(KXO.Models.OmContact.ContactNotes) },
            { nameof(OmContact.ContactOwnerUserId), nameof(KXO.Models.OmContact.ContactOwnerUserId) },
            // No support 2022-07-07  { nameof(OmContact.ContactMonitored), nameof(KXO.Models.OmContact.ContactMonitored) },
            { nameof(OmContact.ContactGuid), nameof(KXO.Models.OmContact.ContactGuid) },
            { nameof(OmContact.ContactLastModified), nameof(KXO.Models.OmContact.ContactLastModified) },
            { nameof(OmContact.ContactCreated), nameof(KXO.Models.OmContact.ContactCreated) },
            // No support 2022-07-07  { nameof(OmContact.ContactBounces), nameof(KXO.Models.OmContact.ContactBounces) },
            { nameof(OmContact.ContactCampaign), nameof(KXO.Models.OmContact.ContactCampaign) },
            // No support 2022-07-07  { nameof(OmContact.ContactSalesForceLeadId), nameof(KXO.Models.OmContact.ContactSalesForceLeadId) },
            // No support 2022-07-07  { nameof(OmContact.ContactSalesForceLeadReplicationDisabled), nameof(KXO.Models.OmContact.ContactSalesForceLeadReplicationDisabled) },
            // No support 2022-07-07  { nameof(OmContact.ContactSalesForceLeadReplicationDateTime), nameof(KXO.Models.OmContact.ContactSalesForceLeadReplicationDateTime) },
            // No support 2022-07-07  { nameof(OmContact.ContactSalesForceLeadReplicationSuspensionDateTime), nameof(KXO.Models.OmContact.ContactSalesForceLeadReplicationSuspensionDateTime) },
            { nameof(OmContact.ContactCompanyName), nameof(KXO.Models.OmContact.ContactCompanyName) },
            // No support 2022-07-07  { nameof(OmContact.ContactSalesForceLeadReplicationRequired), nameof(KXO.Models.OmContact.ContactSalesForceLeadReplicationRequired) },
        };

        // TODO tk: 2022-07-07 replace table data?
        // if (_bulkDataCopyService.CheckIfDataExistsInTargetTable("OM_Contact"))
        // {
        //     _migrationProtocol.Append(HandbookReferences.DataMustNotExistInTargetInstanceTable("OM_Contact"));
        //     _logger.LogError("Data must not exist in target instance table, remove data before proceeding");
        //     return new CommandFailureResult();
        // }

        if (_bulkDataCopyService.CheckForTableColumnsDifferences("OM_Contact", requiredColumnsForContactMigration, out var differences))
        {
            _migrationProtocol.Append(HandbookReferences
                .BulkCopyColumnMismatch("OM_Contact")
                .NeedsManualAction()
                .WithData(differences)
            );
            _logger.LogError("Table {TableName} columns do not match, fix columns before proceeding", "OM_Contact");
            {
                return new CommandFailureResult();
            }
        }

        _primaryKeyMappingContext.PreloadDependencies<CmsUser>(u => u.UserId);
        _primaryKeyMappingContext.PreloadDependencies<CmsState>(u => u.StateId);
        _primaryKeyMappingContext.PreloadDependencies<CmsCountry>(u => u.CountryId);
        

        var bulkCopyRequest = new BulkCopyRequest("OM_Contact",
            s => true,// s => s != "ContactID",
            _ => true, 
            50000,
            requiredColumnsForContactMigration.Keys.ToList(),
            ContactValueInterceptor,
            current => { _logger.LogError("Contact skipped due error, contact: {Contact}", PrintHelper.PrintDictionary(current)); },
            "ContactID"
        );

        _logger.LogTrace("Bulk data copy request: {Request}", bulkCopyRequest);
        _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
        return null;
    }

    private ValueInterceptorResult ContactValueInterceptor(int ordinal, string columnName, object value, Dictionary<string, object?> currentRow)
    {
        if (columnName.Equals(nameof(KXO.Models.OmContact.ContactCompanyName), StringComparison.InvariantCultureIgnoreCase))
        {
            // autofix removed in favor of error report and data consistency
            // var truncatedValue = SqlDataTypeHelper.TruncateString(value, 100);
            // return new ValueInterceptorResult(truncatedValue, true, false);

            if (value is string { Length: > 100 } s)
            {
                _migrationProtocol.Append(HandbookReferences.ValueTruncationSkip("OM_Contact")
                    .WithData(new { value, maxLength = 100, s.Length, columnName, contact = PrintHelper.PrintDictionary(currentRow) })
                );
                return ValueInterceptorResult.SkipRow;
            }
        }

        if (columnName.Equals(nameof(KXO.Models.OmContact.ContactOwnerUserId), StringComparison.InvariantCultureIgnoreCase) && value is int sourceUserId)
        {
            switch (_primaryKeyMappingContext.MapSourceId<CmsUser>(u => u.UserId, sourceUserId))
            {
                case (true, var id):
                    return ValueInterceptorResult.ReplaceValue(id);
                case { Success: false }:
                {
                    _migrationProtocol.Append(HandbookReferences.MissingRequiredDependency<KXO.Models.CmsUser>(columnName, value)
                        .WithData(currentRow));
                    return ValueInterceptorResult.SkipRow;
                }
            }
        }
        
        // if (columnName.Equals(nameof(KXO.Models.OmContact.ContactStateId), StringComparison.InvariantCultureIgnoreCase) && value is int sourceStateId)
        // {
        //     switch (_primaryKeyMappingContext.MapSourceId<CmsState>(u => u.StateId, sourceStateId.NullIfZero()))
        //     {
        //         case (true, var id):
        //             return ValueInterceptorResult.ReplaceValue(id);
        //         case { Success: false }:
        //         {
        //             _migrationProtocol.Append(HandbookReferences.MissingRequiredDependency<KXO.Models.CmsState>(columnName, value)
        //                 .WithData(currentRow));
        //             return ValueInterceptorResult.SkipRow;
        //         }
        //     }
        // }
        //
        // if (columnName.Equals(nameof(KXO.Models.OmContact.ContactCountryId), StringComparison.InvariantCultureIgnoreCase) && value is int sourceCountryId)
        // {
        //     switch (_primaryKeyMappingContext.MapSourceId<CmsCountry>(u => u.CountryId, sourceCountryId.NullIfZero()))
        //     {
        //         case (true, var id):
        //             return ValueInterceptorResult.ReplaceValue(id);
        //         case { Success: false }:
        //         {
        //             _migrationProtocol.Append(HandbookReferences.MissingRequiredDependency<KXO.Models.CmsCountry>(columnName, value)
        //                 .WithData(currentRow));
        //             return ValueInterceptorResult.SkipRow;
        //         }
        //     }
        // }
        
        

        return ValueInterceptorResult.DoNothing;
    }

    #endregion

    #region "Migrate contact activities"

    private CommandResult? MigrateContactActivities(List<int?> migratedSiteIds)
    {
        var requiredColumnsForContactMigration = new Dictionary<string, string>
        {
            { nameof(OmActivity.ActivityId), nameof(KXO.Models.OmActivity.ActivityId) },
            { nameof(OmActivity.ActivityContactId), nameof(KXO.Models.OmActivity.ActivityContactId) },
            { nameof(OmActivity.ActivityCreated), nameof(KXO.Models.OmActivity.ActivityCreated) },
            { nameof(OmActivity.ActivityType), nameof(KXO.Models.OmActivity.ActivityType) },
            // No support 2022-07-07  { nameof(OmActivity.ActivityItemId), nameof(KXO.Models.OmActivity.ActivityItemId) },
            // No support 2022-07-07  { nameof(OmActivity.ActivityItemDetailId), nameof(KXO.Models.OmActivity.ActivityItemDetailId) },
            { nameof(OmActivity.ActivityValue), nameof(KXO.Models.OmActivity.ActivityValue) },
            { nameof(OmActivity.ActivityUrl), nameof(KXO.Models.OmActivity.ActivityUrl) },
            { nameof(OmActivity.ActivityTitle), nameof(KXO.Models.OmActivity.ActivityTitle) },
            { nameof(OmActivity.ActivitySiteId), nameof(KXO.Models.OmActivity.ActivitySiteId) },
            { nameof(OmActivity.ActivityComment), nameof(KXO.Models.OmActivity.ActivityComment) },
            { nameof(OmActivity.ActivityCampaign), nameof(KXO.Models.OmActivity.ActivityCampaign) },
            { nameof(OmActivity.ActivityUrlreferrer), nameof(KXO.Models.OmActivity.ActivityUrlreferrer) },
            { nameof(OmActivity.ActivityCulture), nameof(KXO.Models.OmActivity.ActivityCulture) },
            { nameof(OmActivity.ActivityNodeId), nameof(KXO.Models.OmActivity.ActivityNodeId) },
            { nameof(OmActivity.ActivityUtmsource), nameof(KXO.Models.OmActivity.ActivityUtmsource) },
            // No support 2022-07-07  { nameof(OmActivity.ActivityAbvariantName), nameof(KXO.Models.OmActivity.ActivityAbvariantName) },
            { nameof(OmActivity.ActivityUrlhash), nameof(KXO.Models.OmActivity.ActivityUrlhash) },
            { nameof(OmActivity.ActivityUtmcontent), nameof(KXO.Models.OmActivity.ActivityUtmcontent) },
        };

        // TODO tk: 2022-07-07 replace table data
        // if (_bulkDataCopyService.CheckIfDataExistsInTargetTable("OM_Activity"))
        // {
        //     _migrationProtocol.Append(HandbookReferences.DataMustNotExistInTargetInstanceTable("OM_Activity"));
        //     _logger.LogError("Data must not exist in target instance table, remove data before proceeding");
        //     return new CommandFailureResult();
        // }

        if (_bulkDataCopyService.CheckForTableColumnsDifferences("OM_Activity", requiredColumnsForContactMigration, out var differences))
        {
            _migrationProtocol.Append(HandbookReferences
                .BulkCopyColumnMismatch("OM_Activity")
                .NeedsManualAction()
                .WithData(differences)
            );
            _logger.LogError("Table {TableName} columns do not match, fix columns before proceeding", "OM_Activity");
            {
                return new CommandFailureResult();
            }
        }

        _primaryKeyMappingContext.PreloadDependencies<CmsTree>(u => u.NodeId);
        _primaryKeyMappingContext.PreloadDependencies<OmContact>(u => u.ContactId);

        var bulkCopyRequest = new BulkCopyRequest("OM_Activity",
            s => true,// s => s != "ActivityID",
            reader => migratedSiteIds.Contains(reader.GetInt32(reader.GetOrdinal("ActivitySiteID"))), // TODO tk: 2022-07-07 move condition to source query
            50000,
            requiredColumnsForContactMigration.Keys.ToList(),
            ActivityValueInterceptor,
            current => { _logger.LogError("Contact activity skipped due error, activity: {Activity}", PrintHelper.PrintDictionary(current)); },
            "ActivityID"
        );

        _logger.LogTrace("Bulk data copy request: {Request}", bulkCopyRequest);
        _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
        return null;
    }

    private ValueInterceptorResult ActivityValueInterceptor(int columnOrdinal, string columnName, object value, Dictionary<string, object?> currentRow)
    {
        if (columnName.Equals(nameof(KXO.Models.OmActivity.ActivityContactId), StringComparison.InvariantCultureIgnoreCase) && value is int sourceContactId)
        {
            switch (_primaryKeyMappingContext.MapSourceId<OmContact>(u => u.ContactId, sourceContactId, false))
            {
                case (true, var id):
                    return ValueInterceptorResult.ReplaceValue(id);
                case { Success: false }:
                {
                    _migrationProtocol.Append(HandbookReferences
                        .MissingRequiredDependency<KXO.Models.OmContact>(columnName, value)
                        .WithData(currentRow)
                    );
                    return ValueInterceptorResult.SkipRow;
                }
            }
        }

        if (columnName.Equals(nameof(KXO.Models.OmActivity.ActivitySiteId), StringComparison.InvariantCultureIgnoreCase) &&
            value is int sourceActivitySiteId)
        {
            switch (_primaryKeyMappingContext.MapSourceId<CmsSite>(u => u.SiteId, sourceActivitySiteId.NullIfZero()))
            {
                case (true, var id):
                    return ValueInterceptorResult.ReplaceValue(id ?? 0);
                case { Success: false }:
                {
                    switch (_toolkitConfiguration.UseOmActivitySiteRelationAutofix ?? AutofixEnum.Error)
                    {
                        case AutofixEnum.DiscardData:
                            _logger.LogTrace("Autofix (ActivitySiteId={ActivitySiteId} not exists) => discard data", sourceActivitySiteId);
                            return ValueInterceptorResult.SkipRow;
                        case AutofixEnum.AttemptFix:
                            _logger.LogTrace("Autofix (ActivitySiteId={ActivitySiteId} not exists) => ActivityNodeId=0", sourceActivitySiteId);
                            return ValueInterceptorResult.ReplaceValue(0);
                        default: //error
                            _migrationProtocol.Append(HandbookReferences
                                .MissingRequiredDependency<KXO.Models.CmsSite>(columnName, value)
                                .WithData(currentRow)
                            );
                            return ValueInterceptorResult.SkipRow;
                    }
                }
            }
        }

        if (columnName.Equals(nameof(KXO.Models.OmActivity.ActivityNodeId), StringComparison.InvariantCultureIgnoreCase) && value is int activityNodeId)
        {
            switch (_primaryKeyMappingContext.MapSourceId<CmsTree>(u => u.NodeId, activityNodeId.NullIfZero(), useLocator: false))
            {
                case (true, var id):
                    return ValueInterceptorResult.ReplaceValue(id ?? 0);
                case { Success: false }:
                {
                    switch (_toolkitConfiguration.UseOmActivityNodeRelationAutofix ?? AutofixEnum.Error)
                    {
                        case AutofixEnum.DiscardData:
                            _logger.LogTrace("Autofix (ActivitySiteId={NodeId} not exists) => discard data", activityNodeId);
                            return ValueInterceptorResult.SkipRow;
                        case AutofixEnum.AttemptFix:
                            _logger.LogTrace("Autofix (ActivityNodeId={NodeId} not exists) => ActivityNodeId=0", activityNodeId);
                            return ValueInterceptorResult.ReplaceValue(0);
                        default: //error
                            _migrationProtocol.Append(HandbookReferences
                                .MissingRequiredDependency<KXO.Models.CmsTree>(columnName, value)
                                .WithData(currentRow)
                            );
                            return ValueInterceptorResult.SkipRow;
                    }
                }
            }
        }

        return ValueInterceptorResult.DoNothing;
    }

    #endregion

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}