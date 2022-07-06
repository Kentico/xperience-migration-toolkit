// using Microsoft.Extensions.Logging;
// using Migration.Toolkit.Core.Abstractions;
// using Migration.Toolkit.Core.Contexts;
//
// namespace Migration.Toolkit.Core.Mappers;
//
// public class CmsAclMapper : IEntityMapper<KX13.Models.CmsAcl, KXO.Models.CmsAcl>
// {
//     private readonly ILogger<CmsAclMapper> _logger;
//     private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
//
//     public CmsAclMapper(
//         ILogger<CmsAclMapper> logger,
//         PrimaryKeyMappingContext primaryKeyMappingContext
//     )
//     {
//         _logger = logger;
//         _primaryKeyMappingContext = primaryKeyMappingContext;
//     }
//
//     public IModelMappingResult<KXO.Models.CmsAcl> Map(KX13.Models.CmsAcl? source, KXO.Models.CmsAcl? target)
//     {
//         if (source is null)
//         {
//             _logger.LogTrace("Source entity is not defined.");
//             return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsAcl>().Log(_logger);
//         }
//
//         var newInstance = false;
//         if (target is null)
//         {
//             _logger.LogTrace("Null target supplied, creating new instance.");
//             target = new Migration.Toolkit.KXO.Models.CmsAcl();
//             newInstance = true;
//         }
//         else if (source.Aclguid != target.Aclguid)
//         {
//             // assertion failed
//             _logger.LogTrace("Assertion failed, entity key mismatch.");
//             return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsAcl>().Log(_logger);
//         }
//
//         // target.Aclid = source.Aclid;
//         target.AclinheritedAcls = source.AclinheritedAcls;
//         target.Aclguid = source.Aclguid;
//         target.AcllastModified = source.AcllastModified;
//         target.AclsiteId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsSite>(s => s.SiteId, source.AclsiteId);
//
//         // TODO tk: 2022-05-18 Check DEPS: Aclsite of type CmsSite?
//         // TODO tk: 2022-05-18 Check DEPS: CmsAclitems of type ICollection<CmsAclitem>
//         // TODO tk: 2022-05-18 Check DEPS: CmsTrees of type ICollection<CmsTree>
//
//         return new ModelMappingSuccess<KXO.Models.CmsAcl>(target, newInstance).Log(_logger);
//     }
// }