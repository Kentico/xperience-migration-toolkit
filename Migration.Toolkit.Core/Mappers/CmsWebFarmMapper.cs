// using Microsoft.Extensions.Logging;
// using Migration.Toolkit.Core.Abstractions;
//
// namespace Migration.Toolkit.Core.MigrateWebFarms;
//
// public class CmsWebFarmMapper: IEntityMapper<KX13.Models.CmsWebFarmServer, KXO.Models.CmsWebFarmServer>
// {
//     private readonly ILogger<CmsWebFarmMapper> _logger;
//
//     public CmsWebFarmMapper(ILogger<CmsWebFarmMapper> logger)
//     {
//         _logger = logger;
//     }
//     
//     public ModelMappingResult<KXO.Models.CmsWebFarmServer> Map(KX13.Models.CmsWebFarmServer? source, KXO.Models.CmsWebFarmServer? target)
//     {
//         if (source is null)
//         {
//             _logger.LogTrace("Source entity is not defined.");
//             return new ModelMappingFailedSourceNotDefined<Migration.Toolkit.KXO.Models.CmsWebFarmServer>();
//         }
//
//         var newInstance = false;
//         if (target is null)
//         {
//             _logger.LogTrace("Null target supplied, creating new instance.");
//             target = new Migration.Toolkit.KXO.Models.CmsWebFarmServer();
//             newInstance = true;
//         }
//         else if (source.ServerGuid != target.ServerGuid)
//         {
//             // assertion failed
//             _logger.LogTrace("Assertion failed, entity key mismatch.");
//             return new ModelMappingFailedKeyMismatch<Migration.Toolkit.KXO.Models.CmsWebFarmServer>();
//         }
//
//         // do not try to insert pk
//         // target.ServerId = source.ServerId;
//         target.ServerDisplayName = source.ServerDisplayName;
//         target.ServerName = source.ServerName;
//         target.ServerGuid = source.ServerGuid;
//         target.ServerLastModified = source.ServerLastModified;
//         target.ServerEnabled = source.ServerEnabled;
//         
//         //target.CmsWebFarmServerTasks = source.CmsWebFarmServerTasks;
//        
//         return new ModelMappingSuccess<KXO.Models.CmsWebFarmServer>(target, newInstance);
//
//         // removed in kxo
//         // target.IsExternalWebAppServer = source.IsExternalWebAppServer;
//     }
// }