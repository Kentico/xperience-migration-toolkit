using CMS.DocumentEngine;
using CMS.DocumentEngine.Internal;
using CMS.Helpers;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.Core.Services;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public class TreeNodeMapper: IEntityMapper<KX13.Models.CmsTree, TreeNode>
{
    private readonly ILogger<TreeNodeMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly CoupledDataService _coupledDataService;

    public TreeNodeMapper(ILogger<TreeNodeMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext, CoupledDataService coupledDataService)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _coupledDataService = coupledDataService;
    }
    
    public IModelMappingResult<TreeNode> Map(CmsTree? source, TreeNode? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<TreeNode>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = TreeNode.New(source.NodeClass.ClassName);
            newInstance = true;
        }
        else if (source.NodeGuid != target.NodeGUID)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<TreeNode>().Log(_logger);
        }

        target.NodeGUID = source.NodeGuid;
        target.NodeAlias = source.NodeAlias;
        // target.NodeLevel = source.NodeLevel;
        target.NodeName = source.NodeName;
        // target.NodeOrder = source.NodeOrder;
        
        // TODO tk: 2022-07-04 solve how to produce meaningful failures directly to handbook
        if (_primaryKeyMappingContext.TryRequireMapFromSource<K13M.CmsUser>(u => u.UserId, source.NodeOwner ?? -1, out var targetNodeOwnerId))
        {
            target.NodeOwner = targetNodeOwnerId;
        }
        else
        {
            return new ModelMappingFailedMissingDependencyInTargetInstance<TreeNode>(nameof(target.NodeOwner), source.NodeOwner, HandbookReferences.CmsTreeUserIsMissingInTargetInstance);
        }
        
        // target.NodeAliasPath = source.NodeAliasPath;
        // target.NodeClassName = source.NodeClass.ClassName;
        var customNodeData = new ContainerCustomData();
        customNodeData.LoadData(source.NodeCustomData);
        foreach (var columnName in customNodeData.ColumnNames)
        {
            target.NodeCustomData.SetValue(columnName, customNodeData.GetValue(columnName));
        }

        // target.NodeHasChildren = source.NodeHasChildren;
        // target.NodeHasLinks = source.NodeHasLinks;
        // target.NodeID = source.NodeId;
        // target.NodeSiteName =
        target.NodeParentID = _primaryKeyMappingContext.RequireMapFromSource<K13M.CmsTree>(u => u.NodeId, source.NodeParentId ?? -1);
        // target.NodeSiteID =
        // TODO tk: 2022-06-30 guard linked node id
        // target.NodeLinkedNodeID = ;
        // target.NodeOriginalNodeID = ;
        // TODO tk: 2022-06-30 if different from current site, just skip
        // target.NodeLinkedNodeSiteID = ;

        var selectedCulture = "en-US"; // TODO tk: 2022-06-30 get from configuration
        var sourceDocument = source.CmsDocuments.Single(x => x.DocumentCulture == selectedCulture);
        target.DocumentCulture = sourceDocument.DocumentCulture;
        // TODO tk: 2022-06-30 map content
        // target.DocumentContent = sourceDocument.DocumentContent;
        target.DocumentName = sourceDocument.DocumentName;
        // target.DocumentCreatedWhen = sourceDocument.DocumentCreatedWhen;

        var customDocumentData = new ContainerCustomData();
        customDocumentData.LoadData(sourceDocument.DocumentCustomData);
        foreach (var columnName in customDocumentData.ColumnNames)
        {
            target.DocumentCustomData.SetValue(columnName, customDocumentData.GetValue(columnName));
        }

        // target.DocumentID = sourceDocument.DocumentId;
        // target.DocumentIsArchived = sourceDocument.DocumentIsArchived;
        // target.DocumentLastPublished = sourceDocument.DocumentLastPublished;
        // target.DocumentModifiedWhen = sourceDocument.DocumentModifiedWhen;
        target.DocumentPublishFrom = sourceDocument.DocumentPublishFrom.GetValueOrDefault();
        target.DocumentPublishTo = sourceDocument.DocumentPublishTo.GetValueOrDefault();
        target.DocumentSearchExcluded = sourceDocument.DocumentSearchExcluded.GetValueOrDefault();
        // TODO tk: 2022-06-30  target.DocumentsOnPath
        // target.DocumentCheckedOutWhen = sourceDocument.DocumentCheckedOutWhen;
        // target.DocumentLastVersionName = sourceDocument.DocumentLastVersionNumber;
        // target.DocumentNodeID = sourceDocument.DocumentNodeId;
        // target.DocumentWorkflowActionStatus = sourceDocument.DocumentWorkflowActionStatus;
        target.DocumentGUID = sourceDocument.DocumentGuid.GetValueOrDefault();
        // target.DocumentWorkflowStepID = sourceDocument.DocumentWorkflowStepId;
        // target.DocumentCreatedByUserID = sourceDocument.DocumentCreatedByUserId;
        // target.DocumentModifiedByUserID = sourceDocument.DocumentModifiedByUserId;
        // target.DocumentPublishedVersionHistoryID = sourceDocument.DocumentPublishedVersionHistoryId;
        // target.DocumentCheckedOutByUserID = sourceDocument.DocumentCreatedByUserId;
        target.DocumentWorkflowCycleGUID = sourceDocument.DocumentWorkflowCycleGuid.GetValueOrDefault();
        // target.CoupledClassIDColumn = 

        // Set coupled data
        var fieldsInfo = new DocumentFieldsInfo(source.NodeClass.ClassName);
        if (source.NodeClass.ClassIsCoupledClass)
        {
            var coupledDataRow = _coupledDataService.GetSourceCoupledDataRow(source.NodeClass?.ClassTableName, fieldsInfo.TypeInfo.IDColumn,
                sourceDocument.DocumentForeignKeyValue);
            if (coupledDataRow != null)
            {
                foreach (var (key, value) in coupledDataRow)
                {
                    if (key != fieldsInfo.TypeInfo.IDColumn)
                    {
                        target.SetValue(key, value);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Coupled data is missing for source document {documentId} of class {className}", sourceDocument.DocumentId,
                    source.NodeClass.ClassName);
            }
        }

        return new ModelMappingSuccess<TreeNode>(target, newInstance);
    }
}