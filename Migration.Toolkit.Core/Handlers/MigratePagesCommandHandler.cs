using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Linq;
using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Mappers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services.BulkCopy;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KX13.Models;
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

// TODO tk: 2022-07-06 PAGE URL MAPPINGS
// TODO tk: 2022-07-06 check if set value can overwrite last modified by, etc...
public class MigratePagesCommandHandler : IRequestHandler<MigratePagesCommand, CommandResult>, IDisposable
{
    private readonly ILogger<MigratePagesCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<CmsTreeMapperSource, TreeNode> _nodeMapper;
    private readonly IEntityMapper<CmsPageUrlPath, KXO.Models.CmsPageUrlPath> _pageUrlPathMapper;

    private readonly BulkDataCopyService _bulkDataCopyService;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;
    private readonly KxoPageFacade _pageFacade;
    private readonly PageMigrationContext _pageMigrationContext;

    private KxoContext _kxoContext;

    public MigratePagesCommandHandler(
        ILogger<MigratePagesCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<CmsTreeMapperSource, TreeNode> nodeMapper,
        IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath> pageUrlPathMapper,
        BulkDataCopyService bulkDataCopyService,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol,
        KxoPageFacade pageFacade,
        PageMigrationContext pageMigrationContext
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _nodeMapper = nodeMapper;
        _pageUrlPathMapper = pageUrlPathMapper;
        _bulkDataCopyService = bulkDataCopyService;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _pageFacade = pageFacade;
        _pageMigrationContext = pageMigrationContext;
        _kxoContext = kxoContextFactory.CreateDbContext();
    }

    private HashSet<string> _noLongerSupportedPageTypes = new(new[]
    {
        // "CMS.Root",
        // "CMS.RSSTransformations",
        // "CMS.PagerTransformations",
        // "CMS.Folder",
        "CMS.File",
        // "CMS.KBArticle",
        // "CMS.ImageGallery",
        // "CMS.News",
        // "CMS.MenuItem",
        // "CMS.Article",
        // "CMS.Blog",
        // "CMS.Job",
        // "CMS.SimpleArticle",
        // "CMS.Event",
        // "CMS.PressRelease",
        // "CMS.Faq",
        // "CMS.Office",
        // "CMS.BlogMonth",
        // "CMS.Product",
        // "CMS.BlogPost",
        // "CMS.BookingEvent",
    });

    private record PageUrlPathKey(int PageUrlPathSiteId, string PageUrlPathUrlPathHash, string PageUrlPathCulture);
    
    private KX13M.CmsTree? GetFullSourceCmsTree(int siteId, string cultureCode, Guid nodeGuid)
    {
        using var kx13Context = _kx13ContextFactory.CreateDbContext();

        return kx13Context.CmsTrees
            .Include(t => t.NodeParent)
            .Include(t => t.CmsDocuments.Where(x => x.DocumentCulture == cultureCode))
            .Include(t => t.NodeClass)
            .Include(t => t.CmsPageUrlPaths)
            .Include(t => t.NodeLinkedNode)
            .Where(x => x.NodeSiteId == siteId && x.NodeGuid == nodeGuid)
            .OrderBy(t => t.NodeLevel)
            .ThenBy(t => t.NodeParentId)
            .ThenBy(t => t.NodeId)
            .AsNoTracking()
            .SingleOrDefault();
    }

    public async Task<CommandResult> Handle(MigratePagesCommand request, CancellationToken cancellationToken)
    {
        var cultureCode = request.CultureCode;

        var siteMappings = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId);
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        var classEntityConfiguration = _toolkitConfiguration.EntityConfigurations.GetEntityConfiguration<KX13.Models.CmsClass>();

        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var kx13CmsTrees = kx13Context.CmsTrees
            .Include(t => t.NodeParent)
            .Include(t => t.CmsDocuments.Where(x => x.DocumentCulture == cultureCode))
            .Include(t => t.NodeClass)
            .Include(t => t.CmsPageUrlPaths)
            .Include(t => t.NodeLinkedNode)
            .Where(x => migratedSiteIds.Contains(x.NodeSiteId))
            .OrderBy(t => t.NodeLevel)
            .ThenBy(t => t.NodeParentId)
            .ThenBy(t => t.NodeId)
            .AsNoTrackingWithIdentityResolution();
            // TODO tk: 2022-05-18  .Where(x=>x.IsPublished)
            ;
            
        foreach (var kx13CmsTreeOriginal in kx13CmsTrees)
        {
            _migrationProtocol.FetchedSource(kx13CmsTreeOriginal);
            
            var kx13CmsTree = kx13CmsTreeOriginal;
            var migrationOfLinkedNode = false;
            if (kx13CmsTree.NodeLinkedNode != null)
            {
                if (kx13CmsTree.NodeLinkedNode.NodeSiteId != kx13CmsTree.NodeSiteId)
                {
                    // skip & write to protocol
                    _logger.LogWarning("Linked node with NodeGuid {NodeGuid} is linked from different site - unable to migrate.", kx13CmsTreeOriginal.NodeGuid);
                    _migrationProtocol.Warning(HandbookReferences.CmsTreeTreeIsLinkFromDifferentSite, kx13CmsTree);
                    continue;
                }

                // materialize linked node & write to protocol
                var linkedNode = GetFullSourceCmsTree(kx13CmsTree.NodeSiteId, cultureCode, kx13CmsTree.NodeLinkedNode.NodeGuid);
                // kx13CmsTree = EfCoreHelper.Clone(kx13Context, kx13CmsTreeOriginal);
                
                Debug.Assert(kx13CmsTree != null, nameof(kx13CmsTree) + " != null");
                Debug.Assert(linkedNode != null, nameof(linkedNode) + " != null");

                var originalCmsDocument = linkedNode.CmsDocuments.Single();
                originalCmsDocument.DocumentGuid = Guid.NewGuid();
                originalCmsDocument.DocumentId = 0;

                kx13CmsTree.CmsDocuments.Add(originalCmsDocument);
                kx13CmsTree.NodeLinkedNodeId = null;
                kx13CmsTree.NodeLinkedNodeSiteId = null;
                migrationOfLinkedNode = true;
                
                _logger.LogTrace("Linked node with NodeGuid {NodeGuid} was materialized", kx13CmsTree.NodeGuid);
            }
            
            var kx13CmsDocument = kx13CmsTree.CmsDocuments.SingleOrDefault();
            Debug.Assert(kx13CmsDocument != null, nameof(kx13CmsDocument) + " != null");
            
            if (classEntityConfiguration.ExcludeCodeNames.Contains(kx13CmsTree.NodeClass.ClassName, StringComparer.InvariantCultureIgnoreCase))
            {
                _migrationProtocol.Warning(HandbookReferences.EntityExplicitlyExcludedByCodeName(kx13CmsTree.NodeClass.ClassName, "PageType"), kx13CmsTree);
                _logger.LogWarning("Page: page of class {ClassName} was skipped => it is explicitly excluded in configuration.", kx13CmsTree.NodeClass.ClassName);
                _pageMigrationContext.AddSkippedPage(kx13CmsDocument);
                continue;    
            }
            
            int? mappedParentNodeId;
            if (kx13CmsTree.NodeClass.ClassName == "CMS.Root")
            {
                var targetSiteId = siteMappings[kx13CmsTree.NodeSiteId] ?? 0; // TODO tk: 2022-06-30 report error
                
                mappedParentNodeId = new DocumentQuery("CMS.Root")
                    .OnSite(new SiteInfoIdentifier(targetSiteId))
                    .Culture(cultureCode)
                    .SingleOrDefault()
                    ?.NodeID;

                if (mappedParentNodeId == null)
                {
                    _logger.LogError("Unable to find target instance page root node.");
                    _migrationProtocol.Fatal(HandbookReferences.CmsTreeTreeRootIsMissing, kx13CmsTree);
                    return new CommandFailureResult();
                }

                _migrationProtocol.Warning(HandbookReferences.CmsTreeTreeRootSkip, kx13CmsTree);
                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsTree>(
                    r => r.NodeId,
                    kx13CmsTree.NodeId,
                    mappedParentNodeId.Value
                );
                _pageMigrationContext.AddSuccessfullyMigratedPage(kx13CmsDocument);

                continue;
            }
            
            var isPublished = _pageFacade.IsPublished(new IsPublishedArgument(
                kx13CmsDocument.DocumentCanBePublished, kx13CmsDocument.DocumentWorkflowStepId,
                kx13CmsDocument.DocumentIsArchived, kx13CmsDocument.DocumentCheckedOutVersionHistoryId,
                kx13CmsDocument.DocumentPublishedVersionHistoryId, kx13CmsDocument.DocumentPublishFrom, kx13CmsDocument.DocumentPublishTo)
            );

            if (!isPublished)
            {
                _logger.LogWarning("Source page {pageGuid} is not published => skipping", kx13CmsDocument.DocumentGuid);
                _migrationProtocol.Warning(HandbookReferences.SourcePageIsNotPublished(kx13CmsDocument.DocumentGuid ?? Guid.Empty), kx13CmsDocument);
                _pageMigrationContext.AddSkippedPage(kx13CmsDocument);
                continue;
            }
            
            mappedParentNodeId = _primaryKeyMappingContext.MapFromSourceOrNull<KX13.Models.CmsTree>(t => t.NodeId, kx13CmsTree.NodeParentId);
            if (mappedParentNodeId == null)
            {
                _migrationProtocol.NeedsManualAction(HandbookReferences.CmsTreeTreeParentIsMissing, new
                {
                    SourceCmsDocumentId = kx13CmsDocument.DocumentId,
                    SourceDocumentGuid = kx13CmsDocument.DocumentGuid,
                    SourcePageName = kx13CmsDocument.DocumentName,
                    SourceParentNodeId = kx13CmsTree.NodeParentId
                });
                _logger.LogWarning("Document with guid '{pageGuid}' has missing parent in target instance", kx13CmsDocument.DocumentGuid);
                continue;
            }

            var kxoTreeNode = new DocumentQuery(kx13CmsTree.NodeClass.ClassName)
                .WithGuid(kx13CmsTree.CmsDocuments.Single().DocumentGuid.GetValueOrDefault())
                .SingleOrDefault();

            _migrationProtocol.FetchedTarget(kxoTreeNode);

            if (migrationOfLinkedNode && kxoTreeNode != null)
            {
                _logger.LogWarning("Linked node is already materialized in target instance, if you want to migrate again delete it in target instance.");
                _migrationProtocol.Append(HandbookReferences.LinkedDataAlreadyMaterializedInTargetInstance.WithData(new { kx13CmsTree.NodeGuid, kx13CmsTree.NodeAliasPath }));
                continue;
            }
            
            var kxoTreeNodeParent = new DocumentQuery()
                .Where("NodeID", QueryOperator.Equals, mappedParentNodeId)
                .Culture(cultureCode)
                .Single();
            
            var source = new CmsTreeMapperSource(kx13CmsTree, cultureCode);
            var mapped = _nodeMapper.Map(source, kxoTreeNode);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success : true } result)
            {
                var (treeNode, newInstance) = result;

                ArgumentNullException.ThrowIfNull(treeNode, nameof(treeNode));

                try
                {
                    var treeProvider = new TreeProvider
                    {
                        UpdateUser = false,
                        UpdateTimeStamps = false,
                        LogEvents = false,
                        LogIntegration = false,
                        UpdatePaths = false,
                    };
                    treeNode.TreeProvider = treeProvider;
                    if (newInstance)
                    {
                        treeNode.Insert(kxoTreeNodeParent, false);
                    }
                    else
                    {
                        treeNode.Update(false);
                    }

                    MigratePageUrlPaths(kx13CmsTree, kx13CmsDocument, cultureCode, treeNode);

                    treeNode.Publish();

                    _migrationProtocol.Success(kx13CmsTree, treeNode, mapped);
                    _logger.LogInformation(newInstance
                        ? $"CmsTree: {treeNode.NodeName} with NodeGuid '{treeNode.NodeGUID}' was inserted"
                        : $"CmsTree: {treeNode.NodeName} with NodeGuid '{treeNode.NodeGUID}' was updated");
                }
                catch (DbUpdateException dbUpdateException) when (
                    dbUpdateException.InnerException is SqlException sqlException &&
                    sqlException.Message.Contains("Cannot insert duplicate key row in object") &&
                    sqlException.Message.Contains("IX_CMS_PageUrlPath_PageUrlPathUrlPathHash_PageUrlPathCulture_PageUrlPathSiteID") &&
                    sqlException.Message.Contains("CMS_PageUrlPath")
                )
                {
                    await _kxoContext.DisposeAsync();
                    _logger.LogError(sqlException, "Failed to migrate page url path, possibly due to duplicated PageUrlPathHash.");
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                    _migrationProtocol.NeedsManualAction(
                        HandbookReferences.CmsUserUserNameConstraintBroken,
                        $"Failed to migrate page. PageNodeGuid: {kx13CmsTree.NodeGuid}. Needs manual migration.",
                        kx13CmsTree,
                        treeNode,
                        mapped
                    );
                    continue;
                }
                catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
                {
                    await _kxoContext.DisposeAsync();
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                    _migrationProtocol.Append(HandbookReferences
                        .ErrorCreatingTargetInstance<TreeNode>(ex)
                        .NeedsManualAction()
                        .WithIdentityPrint(treeNode)
                    );
                    _logger.LogEntitySetError(ex, newInstance, treeNode);
                    continue;
                }

                _primaryKeyMappingContext.SetMapping<KX13.Models.CmsTree>(
                    r => r.NodeId,
                    kx13CmsTree.NodeId,
                    treeNode.NodeID
                );
            }
        }

        return new GenericCommandResult();
    }

    private void MigratePageUrlPaths(CmsTree kx13CmsTree, CmsDocument kx13CmsDocument, string cultureCode, TreeNode treeNode)
    {
        using var kxoContext = _kxoContextFactory.CreateDbContext();
        using var kx13Context = _kx13ContextFactory.CreateDbContext();
        
        var pageUrlPathsByHash =
            kxoContext.CmsPageUrlPaths.Include(x => x.PageUrlPathNode)
                .ToDictionary(x => new PageUrlPathKey(x.PageUrlPathSiteId, x.PageUrlPathUrlPathHash, x.PageUrlPathCulture));

        var kx13PageUrlPaths = kx13Context
            .CmsPageUrlPaths.Where(p => p.PageUrlPathNodeId == kx13CmsTree.NodeId && p.PageUrlPathCulture == kx13CmsDocument.DocumentCulture);

        foreach (var kx13PageUrlPath in kx13PageUrlPaths)
        {
            var pageUrlPathKey = new PageUrlPathKey(treeNode.NodeSiteID, kx13PageUrlPath.PageUrlPathUrlPathHash, treeNode.DocumentCulture);
            
            var exists = PageUrlPathInfoProvider.ProviderObject.Get(kx13PageUrlPath.PageUrlPathGuid) != null;
            if (exists)
            {
                _logger.LogWarning("PageUrlPath skipped, already exists in target instance: {Info}", pageUrlPathKey);
                // TODO tk: 2022-07-08 report
                continue;
            }

            var pageUrlPath = PageUrlPathInfo.New();
            pageUrlPath.PageUrlPathCulture = treeNode.DocumentCulture;
            pageUrlPath.PageUrlPathNodeID = treeNode.NodeID;
            pageUrlPath.PageUrlPathSiteID = treeNode.NodeSiteID;
            pageUrlPath.PageUrlPathGUID = kx13PageUrlPath.PageUrlPathGuid;
            pageUrlPath.PageUrlPathUrlPath = kx13PageUrlPath.PageUrlPathUrlPath;
            pageUrlPath.PageUrlPathUrlPathHash = kx13PageUrlPath.PageUrlPathUrlPathHash;
            pageUrlPath.PageUrlPathLastModified = kx13PageUrlPath.PageUrlPathLastModified;
            if (pageUrlPathsByHash.ContainsKey(pageUrlPathKey))
            {
                _logger.LogWarning("PageUrlPath skipped, already exists in target instance: {Info}", pageUrlPathKey);
                // TODO tk: 2022-07-08 report
                continue;
            }

            PageUrlPathInfoProvider.ProviderObject.Set(pageUrlPath);

            _logger.LogEntitySetAction(true, pageUrlPath);
        }
    }

    // private async Task RequireMigratedCmsAcls(KX13Context kx13Context, List<int?> migratedSiteIds, CancellationToken cancellationToken)
    // {
    //     var kx13CmsAcls = kx13Context.CmsAcls
    //         .Where(x => migratedSiteIds.Contains(x.AclsiteId));
    //
    //     foreach (var kx13CmsAcl in kx13CmsAcls)
    //     {
    //         _migrationProtocol.FetchedSource(kx13CmsAcl);
    //
    //         var kxoCmsAcl = await _kxoContext.CmsAcls
    //             .FirstOrDefaultAsync(x => x.Aclguid == kx13CmsAcl.Aclguid, cancellationToken: cancellationToken);
    //
    //         _migrationProtocol.FetchedTarget(kxoCmsAcl);
    //
    //         var mapped = _aclMapper.Map(kx13CmsAcl, kxoCmsAcl);
    //         _migrationProtocol.MappedTarget(mapped);
    //
    //         switch (mapped)
    //         {
    //             case ModelMappingSuccess<KXO.Models.CmsAcl>(var cmsAcl, var newInstance):
    //                 ArgumentNullException.ThrowIfNull(cmsAcl, nameof(cmsAcl));
    //
    //                 if (newInstance)
    //                 {
    //                     _kxoContext.CmsAcls.Add(cmsAcl);
    //                 }
    //                 else
    //                 {
    //                     _kxoContext.CmsAcls.Update(cmsAcl);
    //                 }
    //
    //                 try
    //                 {
    //                     await _kxoContext.SaveChangesAsync(cancellationToken);
    //
    //                     _migrationProtocol.Success(kx13CmsAcl, cmsAcl, mapped);
    //                     _logger.LogInformation(newInstance
    //                         ? $"CmsAcl: {cmsAcl.Aclguid} was inserted."
    //                         : $"CmsAcl: {cmsAcl.Aclguid} was updated.");
    //                 }
    //                 catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
    //                 {
    //                     throw;
    //                 }
    //
    //                 _primaryKeyMappingContext.SetMapping<KX13.Models.CmsAcl>(
    //                     r => r.Aclid,
    //                     kx13CmsAcl.Aclid,
    //                     cmsAcl.Aclid
    //                 );
    //
    //                 break;
    //             default:
    //                 throw new ArgumentOutOfRangeException(nameof(mapped));
    //         }
    //     }
    // }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}
    
    
// public class MigratePagesCommandHandler_Old : IRequestHandler<MigratePagesCommand, CommandResult>, IDisposable
// {
//     private readonly ILogger<MigratePagesCommandHandler_Old> _logger;
//     private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
//     private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
//     private readonly IEntityMapper<KX13.Models.CmsTree, KXO.Models.CmsTree> _treeMapper;
//     private readonly IEntityMapper<KX13.Models.CmsAcl, KXO.Models.CmsAcl> _aclMapper;
//     private readonly IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath> _pageUrlPathMapper;
//     private readonly BulkDataCopyService _bulkDataCopyService;
//     private readonly ToolkitConfiguration _toolkitConfiguration;
//     private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
//     private readonly IMigrationProtocol _migrationProtocol;
//
//     private KxoContext _kxoContext;
//
//     public MigratePagesCommandHandler_Old(
//         ILogger<MigratePagesCommandHandler_Old> logger,
//         IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
//         IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
//         IEntityMapper<KX13.Models.CmsTree, KXO.Models.CmsTree> treeMapper,
//         IEntityMapper<KX13.Models.CmsAcl, KXO.Models.CmsAcl> aclMapper,
//         IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath> pageUrlPathMapper,
//         BulkDataCopyService bulkDataCopyService,
//         ToolkitConfiguration toolkitConfiguration,
//         PrimaryKeyMappingContext primaryKeyMappingContext,
//         IMigrationProtocol migrationProtocol
//     )
//     {
//         _logger = logger;
//         _kxoContextFactory = kxoContextFactory;
//         _kx13ContextFactory = kx13ContextFactory;
//         _treeMapper = treeMapper;
//         _aclMapper = aclMapper;
//         _pageUrlPathMapper = pageUrlPathMapper;
//         _bulkDataCopyService = bulkDataCopyService;
//         _toolkitConfiguration = toolkitConfiguration;
//         _primaryKeyMappingContext = primaryKeyMappingContext;
//         _migrationProtocol = migrationProtocol;
//         _kxoContext = kxoContextFactory.CreateDbContext();
//     }
//
//     public async Task<CommandResult> Handle(MigratePagesCommand request, CancellationToken cancellationToken)
//     {
//         var cultureCode = request.CultureCode;
//
//         var migratedSiteIds = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
//         
//         await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);
//
//         _logger.LogTrace("Selecting existing documents");
//         // assuming coupled data were migrated too in previous attempts
//         var alreadyExistingDocuments = _kxoContext.CmsDocuments
//             .Include(d => d.DocumentNode)
//             .ThenInclude(t => t.NodeClass)
//             .Select(x => new { x.DocumentForeignKeyValue, x.DocumentNode.NodeClass.ClassId, x.DocumentNode.NodeClass.ClassGuid })
//             .ToLookup(k => k.ClassGuid, v => v.DocumentForeignKeyValue);
//         
//
//         _logger.LogTrace("Selecting classes to migrate");
//         var classesToMigrate = kx13Context.CmsClasses
//             .Where(c => c.ClassIsDocumentType && c.ClassIsCoupledClass)
//             .Select(x => new {x.ClassXmlSchema, x.ClassTableName, x.ClassId, x.ClassGuid})
//             ;
//
//         // TODO tk: 2022-06-12 user FormInfo
//         XNamespace nsSchema = "http://www.w3.org/2001/XMLSchema";
//         XNamespace msSchema = "urn:schemas-microsoft-com:xml-msdata";
//         var coupledDataToMigrate = classesToMigrate.AsEnumerable().Select(x =>
//         {
//             var xDoc = XDocument.Parse(x.ClassXmlSchema);
//             var autoIncrementColumns = xDoc.Descendants(nsSchema + "element").Where(x => x.Attribute(msSchema + "AutoIncrement")?.Value == "true")
//                 .Select(x => x.Attribute("name").Value).ToImmutableHashSet();
//
//
//             var result = (x.ClassTableName, x.ClassGuid, autoIncrementColumns);
//             _logger.LogTrace("Class '{classGuild}' Resolved as: {result}", x.ClassGuid, result);
//             
//             return result;
//         });
//
//         // check if data is present in target tables
//         var anyDataPresent = false;
//         foreach (var (tableName, classGuid, autoIncrementColumns) in coupledDataToMigrate)
//         {
//             if (_bulkDataCopyService.CheckIfDataExistsInTargetTable(tableName))
//             {
//                 _logger.LogWarning("Data exists in target coupled data table '{tableName}' - cannot migrate.", tableName);
//                 // TODO tk: 2022-06-30 protocol
//                 anyDataPresent = true;
//             }
//         }
//
//         if (anyDataPresent)
//         {
//             _logger.LogWarning("Some coupled data synchronization was skipped.");
//         }
//
//         foreach (var (tableName, classGuid, autoIncrementColumns) in coupledDataToMigrate)
//         {
//             var lookup = alreadyExistingDocuments[classGuid];
//             var bulkCopyRequest = new BulkCopyRequest(tableName, s => !autoIncrementColumns.Contains(s),
//                 reader => !lookup.Contains(reader.GetInt32(reader.GetOrdinal(autoIncrementColumns.Single()))), 1500);
//             if (_bulkDataCopyService.CheckIfDataExistsInTargetTable(tableName))
//             {
//                 _logger.LogError("Data exists in target coupled data table '{tableName}' - cannot migrate.", tableName);
//                 continue;
//             }
//
//             if (_bulkDataCopyService.CheckForTableColumnsDifferences(tableName, out var mismatchedColumns))
//             {
//                 _logger.LogError("Cannot continue, tables are incompatible.");
//                 _migrationProtocol.Error(HandbookReferences.BulkCopyColumnMismatch(tableName), mismatchedColumns);
//                 continue;
//             }
//             
//             _logger.LogTrace("Bulk data copy request: {request}", bulkCopyRequest);
//             _bulkDataCopyService.CopyTableToTable(bulkCopyRequest);
//         }
//         
//         // await RequireMigratedCmsAcls(kx13Context, migratedSiteIds, cancellationToken);
//
//         var kx13CmsTrees = kx13Context.CmsTrees
//                 .Include(t => t.CmsDocuments.Where(x => x.DocumentCulture == cultureCode))
//                 .Include(t => t.NodeClass)
//                 .Include(t => t.CmsPageUrlPaths)
//                 .Where(x => migratedSiteIds.Contains(x.NodeSiteId))
//                 .OrderBy(t => t.NodeLevel)
//                 .ThenBy(t => t.NodeParentId)
//                 .ThenBy(t => t.NodeId)
//             // TODO tk: 2022-05-18  .Where(x=>x.IsPublished)
//             ;
//
//         var pageUrlPathsByHash =
//             _kxoContext.CmsPageUrlPaths.Include(x => x.PageUrlPathNode)
//                 .ToDictionary(x => new { x.PageUrlPathSiteId, x.PageUrlPathUrlPathHash, x.PageUrlPathCulture });
//         
//         foreach (var kx13CmsTree in kx13CmsTrees)
//         {
//             _migrationProtocol.FetchedSource(kx13CmsTree);
//
//             var kxoCmsTree = await _kxoContext.CmsTrees
//                     .Include(t => t.CmsDocuments.Where(x => x.DocumentCulture == cultureCode))
//                     .Include(t => t.CmsPageUrlPaths)
//                     .FirstOrDefaultAsync(x => x.NodeGuid == kx13CmsTree.NodeGuid, cancellationToken: cancellationToken)
//                 ;
//
//             _migrationProtocol.FetchedTarget(kxoCmsTree);
//
//             if (kx13CmsTree.NodeAliasPath == "/" && kx13CmsTree.NodeClass.ClassName == "CMS.Root")
//             {
//                 if (kxoCmsTree == null)
//                 {
//                     _migrationProtocol.Fatal(HandbookReferences.CmsTreeTreeRootIsMissing, kx13CmsTree);
//                     throw new Exception("Target tree root node is missing");
//                 }
//
//                 _migrationProtocol.Warning(HandbookReferences.CmsTreeTreeRootSkip, kx13CmsTree);
//                 _primaryKeyMappingContext.SetMapping<KX13.Models.CmsTree>(
//                     r => r.NodeId,
//                     kx13CmsTree.NodeId,
//                     kxoCmsTree.NodeId
//                 );
//                 continue;
//             }
//
//             // TODO tk: 2022-06-30 migration path for internal kentico documents (CMS.File)
//             // TODO tk: 2022-06-30 check for linked documents, materialize linked, skip linked from other sites
//             // TODO tk: 2022-06-30 use API for tree/document creation
//             var newNode = TreeNode.New(kx13CmsTree.NodeClass.ClassName);
//             // TODO tk: 2022-06-30 create mapper for TreeNode object
//
//             // TODO tk: 2022-06-30 check column names (in mapper)
//             // newNode.ColumnNames;
//
//             var mapped = _treeMapper.Map(kx13CmsTree, kxoCmsTree);
//             _migrationProtocol.MappedTarget(mapped);
//
//             switch (mapped)
//             {
//                 case ModelMappingSuccess<KXO.Models.CmsTree>(var cmsTree, var newInstance):
//                     ArgumentNullException.ThrowIfNull(cmsTree, nameof(cmsTree));
//                     
//                     //check PageUrlPath constraint
//                     foreach (var newPageUrlPath in cmsTree.CmsPageUrlPaths)
//                     {
//                         if (pageUrlPathsByHash.TryGetValue(
//                                 new
//                                 {
//                                     newPageUrlPath.PageUrlPathSiteId, newPageUrlPath.PageUrlPathUrlPathHash,
//                                     newPageUrlPath.PageUrlPathCulture
//                                 }, out var targetPath))
//                         {
//                             _logger.LogError("Target PageUrlPath already exists {TargetUrlPathHash} for Site {Site} and culture {Culture}", newPageUrlPath.PageUrlPathUrlPathHash, newPageUrlPath.PageUrlPathSiteId, newPageUrlPath.PageUrlPathCulture);
//                             continue;
//                             // if (targetPath.PageUrlPathNode.NodeGuid == newPageUrlPath.PageUrlPathNode.NodeGuid && targetPath.PageUrlPathUrlPath == newPageUrlPath.PageUrlPathUrlPath)
//                             // {
//                             //     _logger.LogInformation("PageUrlPath was matched by NodeGuid '{NodeGuid}' and PageUrlPathUrlPath '{PageUrlPathUrlPath}'",  newPageUrlPath.PageUrlPathNode.NodeGuid, newPageUrlPath.PageUrlPathUrlPath);
//                             // }
//                         }
//                     }
//                     
//                     
//                     if (newInstance)
//                     {
//                         _kxoContext.CmsTrees.Add(cmsTree);
//                     }
//                     else
//                     {
//                         _kxoContext.CmsTrees.Update(cmsTree);
//                     }
//
//                     try
//                     {
//                         await _kxoContext.SaveChangesAsync(cancellationToken);
//
//                         // self reference satisfaction
//                         if (kx13CmsTree.NodeOriginalNodeId == kx13CmsTree.NodeId)
//                         {
//                             cmsTree.NodeOriginalNodeId = cmsTree.NodeId;
//                             await _kxoContext.SaveChangesAsync(cancellationToken);
//                         }
//                         
//                         _migrationProtocol.Success(kx13CmsTree, cmsTree, mapped);
//                         _logger.LogInformation(newInstance
//                             ? $"CmsTree: {cmsTree.NodeName} with NodeGuid '{cmsTree.NodeGuid}' was inserted"
//                             : $"CmsTree: {cmsTree.NodeName} with NodeGuid '{cmsTree.NodeGuid}' was updated");
//                     }
//                     catch (DbUpdateException dbUpdateException) when (
//                         dbUpdateException.InnerException is SqlException sqlException &&
//                         sqlException.Message.Contains("Cannot insert duplicate key row in object") &&
//                         sqlException.Message.Contains("IX_CMS_PageUrlPath_PageUrlPathUrlPathHash_PageUrlPathCulture_PageUrlPathSiteID") &&
//                         sqlException.Message.Contains("CMS_PageUrlPath")
//                     )
//                     {
//                         //PageUrlPathUrlPathHash, PageUrlPathCulture, PageUrlPathSiteID
//                         await _kxoContext.DisposeAsync();
//                         // TODO tk: 2022-05-18 protocol - request manual migration
//                         _logger.LogError(sqlException, "Failed to migrate page url path, possibly due to duplicated PageUrlPathHash.");
//                         _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);
//                     
//                         _migrationProtocol.NeedsManualAction(
//                             HandbookReferences.CmsUserUserNameConstraintBroken,
//                             $"Failed to migrate page. PageNodeGuid: {kx13CmsTree.NodeGuid}. Needs manual migration.",
//                             kx13CmsTree,
//                             cmsTree,
//                             mapped
//                         );
//                         continue;
//                     }
//                     catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
//                     {
//                         throw;
//                     }
//
//                     _primaryKeyMappingContext.SetMapping<KX13.Models.CmsTree>(
//                         r => r.NodeId,
//                         kx13CmsTree.NodeId,
//                         cmsTree.NodeId
//                     );
//
//                     foreach (var kx13CmsDocument in kx13CmsTree.CmsDocuments)
//                     {
//                         var kxoCmdDocument = cmsTree.CmsDocuments.FirstOrDefault(x => x.DocumentGuid == kx13CmsDocument.DocumentGuid);
//                         if (kxoCmdDocument == null)
//                         {
//                             // TODO tk: 2022-05-18 report inconsistency
//                             _logger.LogWarning("Inconsistency: new cmsDocument should be present, but it isn't. NodeGuid={nodeGuid}", cmsTree.NodeGuid);
//                             continue;
//                         }
//                         
//                         _primaryKeyMappingContext.SetMapping<KX13.Models.CmsDocument>(
//                             r => r.DocumentId,
//                             kx13CmsDocument.DocumentId,
//                             kxoCmdDocument.DocumentId
//                         );    
//                     }
//                     
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException(nameof(mapped));
//             }
//         }
//
//         // TODO tk: 2022-06-08 method cannot be used in current impl, synced documents must be reflected in url path search 
//         // await RequireMigratedCmsPageUrlPaths(cancellationToken, kx13Context, migratedSiteIds);
//         
//         return new GenericCommandResult();
//     }
//
//     // private async Task RequireMigratedCmsAcls(KX13Context kx13Context, List<int?> migratedSiteIds, CancellationToken cancellationToken)
//     // {
//     //     var kx13CmsAcls = kx13Context.CmsAcls
//     //         .Where(x => migratedSiteIds.Contains(x.AclsiteId));
//     //
//     //     foreach (var kx13CmsAcl in kx13CmsAcls)
//     //     {
//     //         _migrationProtocol.FetchedSource(kx13CmsAcl);
//     //
//     //         var kxoCmsAcl = await _kxoContext.CmsAcls
//     //             .FirstOrDefaultAsync(x => x.Aclguid == kx13CmsAcl.Aclguid, cancellationToken: cancellationToken);
//     //
//     //         _migrationProtocol.FetchedTarget(kxoCmsAcl);
//     //
//     //         var mapped = _aclMapper.Map(kx13CmsAcl, kxoCmsAcl);
//     //         _migrationProtocol.MappedTarget(mapped);
//     //
//     //         switch (mapped)
//     //         {
//     //             case ModelMappingSuccess<KXO.Models.CmsAcl>(var cmsAcl, var newInstance):
//     //                 ArgumentNullException.ThrowIfNull(cmsAcl, nameof(cmsAcl));
//     //
//     //                 if (newInstance)
//     //                 {
//     //                     _kxoContext.CmsAcls.Add(cmsAcl);
//     //                 }
//     //                 else
//     //                 {
//     //                     _kxoContext.CmsAcls.Update(cmsAcl);
//     //                 }
//     //
//     //                 try
//     //                 {
//     //                     await _kxoContext.SaveChangesAsync(cancellationToken);
//     //
//     //                     _migrationProtocol.Success(kx13CmsAcl, cmsAcl, mapped);
//     //                     _logger.LogInformation(newInstance
//     //                         ? $"CmsAcl: {cmsAcl.Aclguid} was inserted."
//     //                         : $"CmsAcl: {cmsAcl.Aclguid} was updated.");
//     //                 }
//     //                 catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
//     //                 {
//     //                     throw;
//     //                 }
//     //
//     //                 _primaryKeyMappingContext.SetMapping<KX13.Models.CmsAcl>(
//     //                     r => r.Aclid,
//     //                     kx13CmsAcl.Aclid,
//     //                     cmsAcl.Aclid
//     //                 );
//     //
//     //                 break;
//     //             default:
//     //                 throw new ArgumentOutOfRangeException(nameof(mapped));
//     //         }
//     //     }
//     // }
//
//     public void Dispose()
//     {
//         _kxoContext.Dispose();
//     }
// }