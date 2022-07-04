using System.Collections.Immutable;
using System.Xml.Linq;
using CMS.DataEngine;
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
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateFormsCommandHandler : IRequestHandler<MigrateFormsCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateFormsCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<CmsClass, DataClassInfo> _dataClassMapper;
    private readonly IEntityMapper<KX13.Models.CmsForm, KXO.Models.CmsForm> _cmsFormMapper;
    private readonly KxoFormFacade _kxoFormFacade;
    private readonly KxoClassFacade _kxoClassFacade;
    private readonly BulkDataCopyService _bulkDataCopyService;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    public MigrateFormsCommandHandler(
        ILogger<MigrateFormsCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.CmsClass, DataClassInfo> dataClassMapper,
        IEntityMapper<KX13.Models.CmsForm, KXO.Models.CmsForm> cmsFormMapper,
        KxoFormFacade kxoFormFacade,
        KxoClassFacade kxoClassFacade,
        BulkDataCopyService bulkDataCopyService,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _dataClassMapper = dataClassMapper;
        _cmsFormMapper = cmsFormMapper;
        _kxoFormFacade = kxoFormFacade;
        _kxoClassFacade = kxoClassFacade;
        _bulkDataCopyService = bulkDataCopyService;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = kxoContextFactory.CreateDbContext();
    }

    public async Task<GenericCommandResult> Handle(MigrateFormsCommand request, CancellationToken cancellationToken)
    {
        var explicitSiteIdMapping = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();

        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var cmsClassForms = kx13Context.CmsClasses
            .Include(c => c.CmsForms)
            .Where(x => x.ClassIsForm == true)
            .OrderBy(x => x.ClassId)
            .AsEnumerable();

        foreach (var kx13Class in cmsClassForms)
        {
            _migrationProtocol.FetchedSource(kx13Class);

            // nekontrolujeme
            // if (kx13Class.ClassConnectionString?.ToLowerInvariant() != "cmsconnectionstring" &&
            //     kx13Class.ClassConnectionString != _toolkitConfiguration.SourceConnectionString &&
            //     string.IsNullOrWhiteSpace(kx13Class.ClassConnectionString))
            // {
            //     _migrationProtocol.Warning(HandbookReferences.CmsClassClassConnectionStringIsDifferent, kx13Class);
            //     _logger.LogWarning($"CmsClass: {kx13Class.ClassName} => ClassConnectionString is different from source connection string needs attention!");
            // }

            if (!kx13Class.CmsForms.Any(f => explicitSiteIdMapping.Contains(f.FormSiteId)))
            {
                _logger.LogWarning($"CmsClass: {kx13Class.ClassName} => Class site is not migrated => skipping.");
                continue;
            }

            var kxoDataClass = _kxoClassFacade.GetClass(kx13Class.ClassGuid);
            _migrationProtocol.FetchedTarget(kxoDataClass);

            SaveUsingKxoApi(kx13Class, kxoDataClass);

            foreach (var kx13CmsForm in kx13Class.CmsForms)
            {
                _migrationProtocol.FetchedSource(kx13CmsForm);

                var kxoCmsForm = _kxoContext.CmsForms.FirstOrDefault(f => f.FormGuid == kx13CmsForm.FormGuid);

                _migrationProtocol.FetchedTarget(kxoCmsForm);

                var mapped = _cmsFormMapper.Map(kx13CmsForm, kxoCmsForm);
                _migrationProtocol.MappedTarget(mapped);

                switch (mapped)
                {
                    case ModelMappingSuccess<KXO.Models.CmsForm>(var cmsForm, var newInstance):
                        ArgumentNullException.ThrowIfNull(cmsForm, nameof(cmsForm));

                        _migrationProtocol.Success(kx13Class, cmsForm, mapped);
                        
                        if (newInstance)
                        {
                            _kxoContext.CmsForms.Add(cmsForm);
                        }
                        else
                        {
                            _kxoContext.CmsForms.Update(cmsForm);
                        }

                        await _kxoContext.SaveChangesAsync(cancellationToken);
                        
                        _logger.LogInformation(newInstance
                            ? $"CmsForm: {cmsForm.FormName} was inserted."
                            : $"CmsForm: {cmsForm.FormName} was updated.");

                        _primaryKeyMappingContext.SetMapping<KX13.Models.CmsForm>(
                            r => r.FormId,
                            kx13Class.ClassId,
                            cmsForm.FormId
                        );

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mapped));
                }


                XNamespace nsSchema = "http://www.w3.org/2001/XMLSchema";
                XNamespace msSchema = "urn:schemas-microsoft-com:xml-msdata";
                var xDoc = XDocument.Parse(kx13Class.ClassXmlSchema);
                var autoIncrementColumns = xDoc.Descendants(nsSchema + "element")
                    .Where(x => x.Attribute(msSchema + "AutoIncrement")?.Value == "true")
                    .Select(x => x.Attribute("name").Value).ToImmutableHashSet();


                var result = (kx13Class.ClassTableName, kx13Class.ClassGuid, autoIncrementColumns);
                _logger.LogTrace("Class '{classGuild}' Resolved as: {result}", kx13Class.ClassGuid, result);


                // check if data is present in target tables
                if (_bulkDataCopyService.CheckIfDataExistsInTargetTable(kx13Class.ClassTableName))
                {
                    _logger.LogWarning("Data exists in target coupled data table '{tableName}' - cannot migrate, skipping form data migration.", result.ClassTableName);
                    // TODO tk: 2022-06-01 migrate data manually or delete all data
                    continue;
                }

                var bulkCopyRequest = new BulkCopyRequest(
                    kx13Class.ClassTableName, s => !autoIncrementColumns.Contains(s), reader => true,
                    1500
                );

                _logger.LogTrace("Bulk data copy request: {request}", bulkCopyRequest);
                _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
            }
            // await SaveUsingEntityFramework(cancellationToken, kx13CmsClassesDocumentType, kxoCmsClass, kxoContext);
        }

        return new GenericCommandResult();
    }

    private void SaveUsingKxoApi(CmsClass kx13Class, DataClassInfo kxoDataClass)
    {
        var mapped = _dataClassMapper.Map(kx13Class, kxoDataClass);
        _migrationProtocol.MappedTarget(mapped);

        switch (mapped)
        {
            case ModelMappingSuccess<DataClassInfo>(var dataClassInfo, var newInstance):
                ArgumentNullException.ThrowIfNull(dataClassInfo, nameof(dataClassInfo));

                _kxoClassFacade.SetClass(dataClassInfo);

                _migrationProtocol.Success(kx13Class, dataClassInfo, mapped);

                _logger.LogInformation(newInstance
                    ? $"CmsClass: {dataClassInfo.ClassName} was inserted."
                    : $"CmsClass: {dataClassInfo.ClassName} was updated.");

                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsClass>(
                    r => r.ClassId,
                    kx13Class.ClassId,
                    dataClassInfo.ClassID
                );

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mapped));
        }
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}