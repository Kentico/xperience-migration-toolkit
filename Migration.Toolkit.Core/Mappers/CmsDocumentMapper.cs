// using Microsoft.Extensions.Logging;
// using Migration.Toolkit.Core.Abstractions;
// using Migration.Toolkit.Core.Contexts;
// using Migration.Toolkit.Core.MigrationProtocol;
// using Migration.Toolkit.KXO.Models;
//
// namespace Migration.Toolkit.Core.Mappers;
//
// public class CmsDocumentMapper : EntityMapperBase<KX13.Models.CmsDocument, KXO.Models.CmsDocument>
// {
//     private readonly ILogger<CmsDocumentMapper> _logger;
//     private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
//
//     public CmsDocumentMapper(ILogger<CmsDocumentMapper> logger, PrimaryKeyMappingContext primaryKeyMappingContext, IMigrationProtocol protocol) :
//         base(logger, primaryKeyMappingContext, protocol)
//     {
//         _logger = logger;
//         _primaryKeyMappingContext = primaryKeyMappingContext;
//     }
//
//
//     protected override CmsDocument CreateNewInstance(KX13.Models.CmsDocument source) => new CmsDocument();
//
//     protected override CmsDocument MapInternal(KX13.Models.CmsDocument source, CmsDocument target, bool newInstance, MappingHelper mappingHelper)
//     {
//         if (source is null)
//         {
//             _logger.LogTrace("Source entity is not defined.");
//             return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsDocument>().Log(_logger);
//         }
//
//         var newInstance = false;
//         if (target is null)
//         {
//             _logger.LogTrace("Null target supplied, creating new instance.");
//             target = new Migration.Toolkit.KXO.Models.CmsDocument();
//             newInstance = true;
//         }
//         else if (source.DocumentGuid != target.DocumentGuid)
//         {
//             // assertion failed
//             _logger.LogTrace("Assertion failed, entity key mismatch.");
//             return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsDocument>().Log(_logger);
//         }
//
//         // target.DocumentId = source.DocumentId;
//         target.DocumentName = source.DocumentName;
//         target.DocumentModifiedWhen = source.DocumentModifiedWhen;
//         target.DocumentForeignKeyValue = source.DocumentForeignKeyValue; // destination key is preserved!
//         target.DocumentCreatedWhen = source.DocumentCreatedWhen;
//         target.DocumentCheckedOutWhen = source.DocumentCheckedOutWhen;
//         target.DocumentPublishFrom = source.DocumentPublishFrom;
//         target.DocumentPublishTo = source.DocumentPublishTo;
//         target.DocumentCulture = source.DocumentCulture;
//         // target.DocumentPageTitle = source.DocumentPageTitle;
//         // target.DocumentPageKeyWords = source.DocumentPageKeyWords;
//         // target.DocumentPageDescription = source.DocumentPageDescription;
//         target.DocumentContent = source.DocumentContent;
//         target.DocumentCustomData = source.DocumentCustomData;
//         // target.DocumentTags = source.DocumentTags;
//         target.DocumentLastPublished = source.DocumentLastPublished;
//         target.DocumentSearchExcluded = source.DocumentSearchExcluded;
//         target.DocumentLastVersionNumber = source.DocumentLastVersionNumber;
//         target.DocumentIsArchived = source.DocumentIsArchived;
//         target.DocumentGuid = source.DocumentGuid;
//         target.DocumentWorkflowCycleGuid = source.DocumentWorkflowCycleGuid;
//         // target.DocumentIsWaitingForTranslation = source.DocumentIsWaitingForTranslation;
//         // target.DocumentSkuname = source.DocumentSkuname;
//         // target.DocumentSkudescription = source.DocumentSkudescription;
//         // target.DocumentSkushortDescription = source.DocumentSkushortDescription;
//         target.DocumentWorkflowActionStatus = source.DocumentWorkflowActionStatus;
//         target.DocumentCanBePublished = source.DocumentCanBePublished;
//         target.DocumentPageBuilderWidgets = source.DocumentPageBuilderWidgets;
//         target.DocumentPageTemplateConfiguration = source.DocumentPageTemplateConfiguration;
//         target.DocumentAbtestConfiguration = source.DocumentAbtestConfiguration;
//         target.DocumentShowInMenu = source.DocumentShowInMenu;
//
//         // TODO tk: 2022-05-18 map foreign keys
//         target.DocumentModifiedByUserId =
//             _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.DocumentModifiedByUserId);
//         target.DocumentCreatedByUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.DocumentCreatedByUserId);
//         target.DocumentCheckedOutByUserId =
//             _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.DocumentCheckedOutByUserId);
//         // target.DocumentCheckedOutVersionHistoryId = source.DocumentCheckedOutVersionHistoryId;
//         // target.DocumentPublishedVersionHistoryId = source.DocumentPublishedVersionHistoryId;
//         // target.DocumentWorkflowStepId = source.DocumentWorkflowStepId;
//
//         // node is will be filled by EF on insert
//         // target.DocumentNodeId = source.DocumentNodeId;
//         // target.DocumentTagGroupId = source.DocumentTagGroupId;
//
//         // TODO tk: 2022-05-18 Check DEPS: DocumentCheckedOutByUser of type CmsUser?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentCheckedOutVersionHistory of type CmsVersionHistory?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentCreatedByUser of type CmsUser?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentModifiedByUser of type CmsUser?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentNode of type CmsTree
//         // TODO tk: 2022-05-18 Check DEPS: DocumentPublishedVersionHistory of type CmsVersionHistory?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentTagGroup of type CmsTagGroup?
//         // TODO tk: 2022-05-18 Check DEPS: DocumentWorkflowStep of type CmsWorkflowStep?
//         // TODO tk: 2022-05-18 Check DEPS: CmsAlternativeUrls of type ICollection<CmsAlternativeUrl>
//         // TODO tk: 2022-05-18 Check DEPS: CmsAttachments of type ICollection<CmsAttachment>
//         // TODO tk: 2022-05-18 Check DEPS: Categories of type ICollection<CmsCategory>
//         // TODO tk: 2022-05-18 Check DEPS: Tags of type ICollection<CmsTag>
//
//         return new ModelMappingSuccess<KXO.Models.CmsDocument>(target, newInstance).Log(_logger);
//     }
// }