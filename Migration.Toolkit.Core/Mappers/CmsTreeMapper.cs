// using Microsoft.Extensions.Logging;
// using Migration.Toolkit.Core.Abstractions;
// using Migration.Toolkit.Core.Contexts;
// using Migration.Toolkit.KX13.Models;
//
// namespace Migration.Toolkit.Core.Mappers;
//
// public class CmsTreeMapper: IEntityMapper<KX13.Models.CmsTree, KXO.Models.CmsTree>
// {
//     private readonly ILogger<CmsTreeMapper> _logger;
//     private readonly IEntityMapper<KX13.Models.CmsDocument, KXO.Models.CmsDocument> _documentMapper;
//     private readonly IEntityMapper<CmsPageUrlPath, KXO.Models.CmsPageUrlPath> _cmsPageUrlPathMapper;
//     private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
//
//     public CmsTreeMapper(
//         ILogger<CmsTreeMapper> logger, 
//         IEntityMapper<KX13.Models.CmsDocument, KXO.Models.CmsDocument> documentMapper,
//         IEntityMapper<KX13.Models.CmsPageUrlPath, KXO.Models.CmsPageUrlPath> cmsPageUrlPathMapper,
//         PrimaryKeyMappingContext primaryKeyMappingContext
//         )
//     {
//         _logger = logger;
//         _documentMapper = documentMapper;
//         _cmsPageUrlPathMapper = cmsPageUrlPathMapper;
//         _primaryKeyMappingContext = primaryKeyMappingContext;
//     }
//     
//     public IModelMappingResult<KXO.Models.CmsTree> Map(KX13.Models.CmsTree? source, KXO.Models.CmsTree? target)
//     {
//         if (source is null)
//         {
//             _logger.LogTrace("Source entity is not defined.");
//             return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsTree>().Log(_logger);
//         }
//
//         var newInstance = false;
//         if (target is null)
//         {
//             _logger.LogTrace("Null target supplied, creating new instance.");
//             target = new Migration.Toolkit.KXO.Models.CmsTree();
//             newInstance = true;
//         }
//         else if (source.NodeGuid != target.NodeGuid)
//         {
//             // assertion failed
//             _logger.LogTrace("Assertion failed, entity key mismatch.");
//             return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsTree>().Log(_logger);
//         }
//         
//         // target.NodeId = source.NodeId;
//         target.NodeAliasPath = source.NodeAliasPath;
//         target.NodeName = source.NodeName;
//         target.NodeAlias = source.NodeAlias;
//         target.NodeLevel = source.NodeLevel;
//         target.NodeGuid = source.NodeGuid;
//         target.NodeOrder = source.NodeOrder;
//         target.IsSecuredNode = source.IsSecuredNode;
//         target.NodeCustomData = source.NodeCustomData;
//         target.NodeHasChildren = source.NodeHasChildren;
//         target.NodeHasLinks = source.NodeHasLinks;
//         // target.NodeIsAclowner = source.NodeIsAclowner;
//         
//         foreach (var sourceCmsDocument in source.CmsDocuments)
//         {
//             var targetCmsDocument = target.CmsDocuments.FirstOrDefault(x => x.DocumentGuid == sourceCmsDocument.DocumentGuid);
//             var mapped = _documentMapper.Map(sourceCmsDocument, targetCmsDocument);
//
//             if (mapped is { Success: true, NewInstance: true })
//             {
//                 target.CmsDocuments.Add(mapped.Item!);
//             }
//         }
//         
//         var aggregatedResult = new AggregatedResult<Migration.Toolkit.KXO.Models.CmsTree>(target, newInstance);
//         foreach (var sourceCmsPageUrlPath in source.CmsPageUrlPaths)
//         {
//             // x.PageUrlPathUrlPathHash == kx13CmsPageUrlPath.PageUrlPathUrlPathHash &&
//             // x.PageUrlPathCulture == kx13CmsPageUrlPath.PageUrlPathCulture &&
//             // x.PageUrlPathSiteId == sourceSiteId,
//             // var explicitIdMapping = _primaryKeyMappingContext.MapFromSource<K13M.CmsPageUrlPath>(p => p.PageUrlPathId, sourceCmsPageUrlPath.PageUrlPathId);
//             // if (explicitIdMapping is int pageUrlPathMapping)
//             // {
//             //     pageUrlPathMapping    
//             // }
//             
//             var targetCmsUrlPath = target.CmsPageUrlPaths.FirstOrDefault(x => x.PageUrlPathGuid == sourceCmsPageUrlPath.PageUrlPathGuid);
//             switch (_cmsPageUrlPathMapper.Map(sourceCmsPageUrlPath, targetCmsUrlPath))
//             {
//                 case { Success: true, NewInstance: true } result:
//                 {
//                     // ok
//                     target.CmsPageUrlPaths.Add(result.Item);
//                     break;
//                 }
//                 case { Success: true, NewInstance: false }:
//                 {
//                     // ok
//                     break;
//                 }
//                 case { Success: false } result:
//                 {
//                     aggregatedResult.AddResult(result);
//                     return aggregatedResult.Log(_logger);
//                 }
//             }
//         }
//
//         // TODO tk: 2022-05-18 map foreign keys
//         // target.NodeSkuid = source.NodeSkuid;
//         // target.NodeAclid = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsAcl>(c => c.Aclid, source.NodeAclid);
//         target.NodeOwner = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.NodeOwner);
//         target.NodeClassId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsClass>(c => c.ClassId, source.NodeClassId);
//         target.NodeParentId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsTree>(tree => tree.NodeId, source.NodeParentId);
//         target.NodeSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(site => site.SiteId, source.NodeSiteId);
//         target.NodeLinkedNodeId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsTree>(tree => tree.NodeId, source.NodeLinkedNodeId);
//         target.NodeLinkedNodeSiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(site => site.SiteId, source.NodeLinkedNodeSiteId);
//
//         if (source.NodeId == source.NodeOriginalNodeId)
//         {
//             // TODO tk: 2022-05-19 can be self reference or reference to other document, check => add logic for post insert manipulation
//         }
//         else
//         {
//             target.NodeOriginalNodeId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsTree>(tree => tree.NodeId, source.NodeOriginalNodeId);    
//         }
//         
//
//         // TODO tk: 2022-05-18 Check DEPS: NodeAcl of type CmsAcl?
//         // TODO tk: 2022-05-18 Check DEPS: NodeClass of type CmsClass
//         // TODO tk: 2022-05-18 Check DEPS: NodeLinkedNode of type CmsTree?
//         // TODO tk: 2022-05-18 Check DEPS: NodeLinkedNodeSite of type CmsSite?
//         // TODO tk: 2022-05-18 Check DEPS: NodeOriginalNode of type CmsTree?
//         // TODO tk: 2022-05-18 Check DEPS: NodeOwnerNavigation of type CmsUser?
//         // TODO tk: 2022-05-18 Check DEPS: NodeParent of type CmsTree?
//         // TODO tk: 2022-05-18 Check DEPS: NodeSite of type CmsSite
//         // TODO tk: 2022-05-18 Check DEPS: NodeSku of type ComSku?
//         // TODO tk: 2022-05-18 Check DEPS: CmsDocuments of type ICollection<CmsDocument>
//         // TODO tk: 2022-05-18 Check DEPS: CmsPageFormerUrlPaths of type ICollection<CmsPageFormerUrlPath>
//         // TODO tk: 2022-05-18 Check DEPS: CmsPageUrlPaths of type ICollection<CmsPageUrlPath>
//         // TODO tk: 2022-05-18 Check DEPS: CmsRelationshipLeftNodes of type ICollection<CmsRelationship>
//         // TODO tk: 2022-05-18 Check DEPS: CmsRelationshipRightNodes of type ICollection<CmsRelationship>
//         // TODO tk: 2022-05-18 Check DEPS: ComMultiBuyDiscountTrees of type ICollection<ComMultiBuyDiscountTree>
//
//         return new ModelMappingSuccess<KXO.Models.CmsTree>(target, newInstance).Log(_logger);
//     }
// }