using System.Collections.Immutable;
using System.Diagnostics;
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
        _kxoClassFacade = kxoClassFacade;
        _bulkDataCopyService = bulkDataCopyService;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = kxoContextFactory.CreateDbContext();
    }

    public async Task<GenericCommandResult> Handle(MigrateFormsCommand request, CancellationToken cancellationToken)
    {
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();

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

            if (!kx13Class.CmsForms.Any(f => migratedSiteIds.Contains(f.FormSiteId)))
            {
                _logger.LogWarning($"CmsClass: {kx13Class.ClassName} => Class site is not migrated => skipping.");
                continue;
            }

            var kxoDataClass = _kxoClassFacade.GetClass(kx13Class.ClassGuid);
            _migrationProtocol.FetchedTarget(kxoDataClass);

            var classSuccessFullySaved = MapAndSaveUsingKxoApi(kx13Class, kxoDataClass);
            if (!classSuccessFullySaved)
            {
                continue;
            }
            
            foreach (var kx13CmsForm in kx13Class.CmsForms)
            {
                _migrationProtocol.FetchedSource(kx13CmsForm);
                
                var kxoCmsForm = _kxoContext.CmsForms.FirstOrDefault(f => f.FormGuid == kx13CmsForm.FormGuid);
                
                _migrationProtocol.FetchedTarget(kxoCmsForm);

                var mapped = _cmsFormMapper.Map(kx13CmsForm, kxoCmsForm);
                _migrationProtocol.MappedTarget(mapped);

                if (mapped is { Success : true } result)
                {
                    var (cmsForm, newInstance) = result;
                    ArgumentNullException.ThrowIfNull(cmsForm, nameof(cmsForm));

                    try
                    {
                        if (newInstance)
                        {
                            _kxoContext.CmsForms.Add(cmsForm);
                        }
                        else
                        {
                            _kxoContext.CmsForms.Update(cmsForm);
                        }

                        await _kxoContext.SaveChangesAsync(cancellationToken);
                        _logger.LogEntitySetAction(newInstance, cmsForm);

                        _primaryKeyMappingContext.SetMapping<KX13.Models.CmsForm>(
                            r => r.FormId,
                            kx13Class.ClassId,
                            cmsForm.FormId
                        );
                    }
                    catch (Exception ex)
                    {
                        await _kxoContext.DisposeAsync(); // reset context errors
                        _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                        _migrationProtocol.Append(HandbookReferences
                            .ErrorCreatingTargetInstance<KXOM.CmsForm>(ex)
                            .NeedsManualAction()
                            .WithIdentityPrint(cmsForm)
                        );
                        _logger.LogEntitySetError(ex, newInstance, cmsForm);
                        
                        continue;
                    }
                }

                Debug.Assert(kx13Class.ClassTableName != null, "kx13Class.ClassTableName != null");
                var csi = new ClassStructureInfo(kx13Class.ClassXmlSchema, kx13Class.ClassXmlSchema, kx13Class.ClassTableName);
                
                XNamespace nsSchema = "http://www.w3.org/2001/XMLSchema";
                XNamespace msSchema = "urn:schemas-microsoft-com:xml-msdata";
                var xDoc = XDocument.Parse(kx13Class.ClassXmlSchema);
                var autoIncrementColumns = xDoc.Descendants(nsSchema + "element")
                    .Where(x => x.Attribute(msSchema + "AutoIncrement")?.Value == "true")
                    .Select(x => x.Attribute("name").Value).ToImmutableHashSet();

                Debug.Assert(autoIncrementColumns.Count == 1, "autoIncrementColumns.Count == 1");
                // TODO tk: 2022-07-08 not true : autoIncrementColumns.First() == csi.IDColumn
                // Debug.Assert(autoIncrementColumns.First() == csi.IDColumn, "autoIncrementColumns.First() == csi.IDColumn");

                var r = (kx13Class.ClassTableName, kx13Class.ClassGuid, autoIncrementColumns);
                _logger.LogTrace("Class '{ClassGuild}' Resolved as: {Result}", kx13Class.ClassGuid, r);
                
                // check if data is present in target tables
                if (_bulkDataCopyService.CheckIfDataExistsInTargetTable(kx13Class.ClassTableName))
                {
                    _logger.LogWarning("Data exists in target coupled data table '{TableName}' - cannot migrate, skipping form data migration", r.ClassTableName);
                    _migrationProtocol.Append(HandbookReferences
                        .DataMustNotExistInTargetInstanceTable(kx13Class.ClassTableName)
                    );
                    continue;
                }

                var bulkCopyRequest = new BulkCopyRequest(
                    kx13Class.ClassTableName, s => !autoIncrementColumns.Contains(s), reader => true,
                    20000
                );

                _logger.LogTrace("Bulk data copy request: {Request}", bulkCopyRequest);
                _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
            }
        }

        return new GenericCommandResult();
    }

    private bool MapAndSaveUsingKxoApi(CmsClass kx13Class, DataClassInfo kxoDataClass)
    {
        var mapped = _dataClassMapper.Map(kx13Class, kxoDataClass);
        _migrationProtocol.MappedTarget(mapped);

        if (mapped is { Success : true } result)
        {
            var (dataClassInfo, newInstance) = result;
            ArgumentNullException.ThrowIfNull(dataClassInfo, nameof(dataClassInfo));

            try
            {
                _kxoClassFacade.SetClass(dataClassInfo);

                _migrationProtocol.Success(kx13Class, dataClassInfo, mapped);
                _logger.LogEntitySetAction(newInstance, dataClassInfo);

                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsClass>(
                    r => r.ClassId,
                    kx13Class.ClassId,
                    dataClassInfo.ClassID
                );

                return true;
            }
            catch (Exception ex)
            {
                _migrationProtocol.Append(HandbookReferences
                    .ErrorCreatingTargetInstance<DataClassInfo>(ex)
                    .NeedsManualAction()
                    .WithIdentityPrint(dataClassInfo)
                );
                _logger.LogEntitySetError(ex, newInstance, dataClassInfo);
            }
        }
        
        return false;
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}