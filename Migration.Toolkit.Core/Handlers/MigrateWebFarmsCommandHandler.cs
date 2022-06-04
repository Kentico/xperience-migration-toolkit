// using MediatR;
// using Microsoft.Data.SqlClient;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Migration.Toolkit.Core.Abstractions;
// using Migration.Toolkit.Core.Contexts;
// using Migration.Toolkit.Core.MigrationProtocol;
// using Migration.Toolkit.KX13.Context;
// using Migration.Toolkit.KXO.Context;
//
// namespace Migration.Toolkit.Core.MigrateUsers;
//
// public class MigrateWebFarmsCommandHandler: IRequestHandler<MigrateWebFarmsCommand, GenericCommandResult>, IDisposable
// {
//     private readonly ILogger<MigrateWebFarmsCommandHandler> _logger;
//     private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
//     private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
//     private readonly IEntityMapper<KX13.Models.CmsWebFarmServer, KXO.Models.CmsWebFarmServer> _webFarmMapper;
//     private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
//     private readonly IMigrationProtocol _migrationProtocol;
//
//     private KxoContext _kxoContext;
//
//     public MigrateWebFarmsCommandHandler(
//         ILogger<MigrateWebFarmsCommandHandler> logger,
//         IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
//         IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
//         IEntityMapper<KX13.Models.CmsWebFarmServer, KXO.Models.CmsWebFarmServer> webFarmMapper,
//         PrimaryKeyMappingContext primaryKeyMappingContext,
//         IMigrationProtocol migrationProtocol
//     )
//     {
//         _logger = logger;
//         _kxoContextFactory = kxoContextFactory;
//         _kx13ContextFactory = kx13ContextFactory;
//         _webFarmMapper = webFarmMapper;
//         _primaryKeyMappingContext = primaryKeyMappingContext;
//         _migrationProtocol = migrationProtocol;
//         _kxoContext = _kxoContextFactory.CreateDbContext();
//     }
//     
//     public async Task<GenericCommandResult> Handle(MigrateWebFarmsCommand request, CancellationToken cancellationToken)
//     {
//         using var protocolScope = _migrationProtocol.CreateScope<MigrateWebFarmsCommandHandler>();  
//         
//         await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);
//
//         foreach (var kx13WebFarm in kx13Context.CmsWebFarmServers)
//         {
//             _migrationProtocol.FetchedSource(kx13WebFarm);
//             _logger.LogTrace("Migrating web farm server {serverName} with ServerGuid {serverGuid}", kx13WebFarm.ServerName, kx13WebFarm.ServerGuid);
//             
//             var kxoWebFarm = await _kxoContext.CmsWebFarmServers.FirstOrDefaultAsync(farm => farm.ServerGuid == kx13WebFarm.ServerGuid, cancellationToken);
//             _migrationProtocol.FetchedTarget(kxoWebFarm);
//
//             var mapped = _webFarmMapper.Map(kx13WebFarm, kxoWebFarm);
//             _migrationProtocol.MappedTarget(mapped);
//             mapped.LogResult(_logger);
//
//             switch (mapped)
//             {
//                 case ModelMappingSuccess<KXO.Models.CmsWebFarmServer>(var cmsWebFarmServer, var newInstance):
//                     ArgumentNullException.ThrowIfNull(cmsWebFarmServer, nameof(cmsWebFarmServer));
//
//                     if (newInstance)
//                     {
//                         _kxoContext.CmsWebFarmServers.Add(cmsWebFarmServer);
//                     }
//                     else
//                     {
//                         _kxoContext.CmsWebFarmServers.Update(cmsWebFarmServer);
//                     }
//
//                     try
//                     {
//                         await _kxoContext.SaveChangesAsync(cancellationToken);
//
//                         _migrationProtocol.Success<KX13.Models.CmsWebFarmServer, KXO.Models.CmsWebFarmServer>(kx13WebFarm, cmsWebFarmServer, mapped);
//                         _logger.LogInformation(newInstance
//                             ? $"CmsWebFarmServer: {cmsWebFarmServer.ServerName} with ServerGuid '{cmsWebFarmServer.ServerGuid}' was inserted."
//                             : $"CmsWebFarmServer: {cmsWebFarmServer.ServerName} with ServerGuid '{cmsWebFarmServer.ServerGuid}' was updated.");
//                     }
//                     catch (DbUpdateException dbUpdateException) when (
//                         dbUpdateException.InnerException is SqlException sqlException &&
//                         sqlException.Message.Contains("Cannot insert duplicate key row in object")
//                     )
//                     {
//                         await _kxoContext.DisposeAsync();
//                         _logger.LogError("Failed to migrate web farm server - server guid: {srverGuid}. Use needs manual migration. Server name: {serverName}", kx13WebFarm.ServerGuid, kx13WebFarm.ServerName);
//                         _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);
//
//                         _migrationProtocol.NeedsManualAction(
//                             HandbookReferences.CmsWebFarmServerSkip,
//                             $"Failed to migrate web farm server - server guid: {kx13WebFarm.ServerGuid}. Use needs manual migration. Server name: {kx13WebFarm.ServerName}",
//                             kx13WebFarm,
//                             cmsWebFarmServer,
//                             mapped
//                         );
//                         continue;
//                     }
//
//                     _primaryKeyMappingContext.SetMapping<KX13.Models.CmsWebFarmServer>(r => r.ServerId, kx13WebFarm.ServerId, cmsWebFarmServer.ServerId);
//
//                     break;
//                 default:
//                     break;
//             }
//         }
//
//         return new GenericCommandResult();
//     }
//
//     public void Dispose()
//     {
//         _kxoContext.Dispose();
//     }
// }