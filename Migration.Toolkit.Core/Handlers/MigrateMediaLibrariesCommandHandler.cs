using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateMediaLibrariesCommandHandler : IRequestHandler<MigrateMediaLibrariesCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateMediaLibrariesCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<KX13.Models.MediaLibrary, KXO.Models.MediaLibrary> _mediaLibraryMapper;
    private readonly IEntityMapper<KX13.Models.MediaFile, KXO.Models.MediaFile> _mediaFileMapper;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    public MigrateMediaLibrariesCommandHandler(
        ILogger<MigrateMediaLibrariesCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.MediaLibrary, KXO.Models.MediaLibrary> mediaLibraryMapper,
        IEntityMapper<KX13.Models.MediaFile, KXO.Models.MediaFile> mediaFileMapper,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _mediaLibraryMapper = mediaLibraryMapper;
        _mediaFileMapper = mediaFileMapper;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = kxoContextFactory.CreateDbContext();
    }

    public async Task<GenericCommandResult> Handle(MigrateMediaLibrariesCommand request, CancellationToken cancellationToken)
    {
        // var (dry, cultureCode) = request;
        
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);
        
        var explicitSiteIdMapping = _toolkitConfiguration.RequireSiteIdExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        // TODO tk: 2022-05-19 reorder method arguments
        // await RequireMigratedCmsAcls(cancellationToken, kx13Context);

        var kx13MediaLibraries = kx13Context.MediaLibraries
                .Where(x => explicitSiteIdMapping.Contains(x.LibrarySiteId))
                .OrderBy(t => t.LibraryId)
            ;

        foreach (var kx13MediaLibrary in kx13MediaLibraries)
        {
            _migrationProtocol.FetchedSource(kx13MediaLibrary);

            var kxoCmsTree = await _kxoContext.MediaLibraries
                .FirstOrDefaultAsync(x => x.LibraryGuid == kx13MediaLibrary.LibraryGuid, cancellationToken: cancellationToken);

            _migrationProtocol.FetchedTarget(kxoCmsTree);

            // TODO tk: 2022-05-20 any reasons why media library shouldn't be migrated 
            
            var mapped = _mediaLibraryMapper.Map(kx13MediaLibrary, kxoCmsTree);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case ModelMappingSuccess<KXO.Models.MediaLibrary>(var cmsMediaLibrary, var newInstance):
                    ArgumentNullException.ThrowIfNull(cmsMediaLibrary, nameof(cmsMediaLibrary));
                    
                    if (newInstance)
                    {
                        _kxoContext.MediaLibraries.Add(cmsMediaLibrary);
                    }
                    else
                    {
                        _kxoContext.MediaLibraries.Update(cmsMediaLibrary);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success(kx13MediaLibrary, cmsMediaLibrary, mapped);
                        _logger.LogInformation(newInstance
                            ? $"MediaLibrary: {cmsMediaLibrary.LibraryName} with NodeGuid '{cmsMediaLibrary.LibraryGuid}' was inserted."
                            : $"MediaLibrary: {cmsMediaLibrary.LibraryName} with NodeGuid '{cmsMediaLibrary.LibraryGuid}' was updated.");
                    }
                    catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
                    {
                        throw;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.MediaLibrary>(
                        r => r.LibraryId,
                        kx13MediaLibrary.LibraryId,
                        cmsMediaLibrary.LibraryId
                    );

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }
        
        // TODO tk: 2022-05-19 reorder method arguments
        await RequireMigratedMediaFiles(cancellationToken, kx13Context, explicitSiteIdMapping);
        
        
        return new GenericCommandResult();
    }

    private async Task RequireMigratedMediaFiles(CancellationToken cancellationToken, KX13Context kx13Context, List<int?> explicitSiteIdMapping)
    {
        var kx13MediaFiles = kx13Context.MediaFiles
            .Where(x => explicitSiteIdMapping.Contains(x.FileSiteId));

        foreach (var kx13MediaFile in kx13MediaFiles)
        {
            _migrationProtocol.FetchedSource(kx13MediaFile);

            var kxoCmsAcl = await _kxoContext.MediaFiles
                .FirstOrDefaultAsync(x => x.FileGuid == kx13MediaFile.FileGuid, cancellationToken: cancellationToken);

            _migrationProtocol.FetchedTarget(kxoCmsAcl);

            var mapped = _mediaFileMapper.Map(kx13MediaFile, kxoCmsAcl);
            _migrationProtocol.MappedTarget(mapped);

            switch (mapped)
            {
                case ModelMappingSuccess<KXO.Models.MediaFile>(var mediaFile, var newInstance):
                    ArgumentNullException.ThrowIfNull(mediaFile, nameof(mediaFile));

                    if (newInstance)
                    {
                        _kxoContext.MediaFiles.Add(mediaFile);
                    }
                    else
                    {
                        _kxoContext.MediaFiles.Update(mediaFile);
                    }

                    try
                    {
                        await _kxoContext.SaveChangesAsync(cancellationToken);

                        _migrationProtocol.Success(kx13MediaFile, mediaFile, mapped);
                        _logger.LogInformation(newInstance
                            ? $"MediaFile: {mediaFile.FileGuid} was inserted."
                            : $"MediaFile: {mediaFile.FileGuid} was updated.");
                    }
                    catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
                    {
                        throw;
                    }

                    _primaryKeyMappingContext.SetMapping<KX13.Models.MediaFile>(
                        r => r.FileId,
                        kx13MediaFile.FileId,
                        mediaFile.FileId
                    );

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mapped));
            }
        }
    }

    // TODO tk: 2022-05-19 might be better to migrate after each cmsTree in case migration get interrupted
    // private async Task RequireMigratedCmsPageUrlPaths(CancellationToken cancellationToken, KX13Context kx13Context)
    // {
    //     var kx13CmsPageUrlPaths = kx13Context.CmsPageUrlPaths
    //         .Where(x => _globalConfiguration.SiteIdMapping.Keys.Contains(x.PageUrlPathSiteId));
    //
    //     foreach (var kx13CmsPageUrlPath in kx13CmsPageUrlPaths)
    //     {
    //         _migrationProtocol.FetchedSource(kx13CmsPageUrlPath);
    //
    //         var kxoCmsPageUrlPaths = await _kxoContext.CmsPageUrlPaths
    //             .FirstOrDefaultAsync(x => x.PageUrlPathGuid == kx13CmsPageUrlPath.PageUrlPathGuid, cancellationToken: cancellationToken);
    //
    //         _migrationProtocol.FetchedTarget(kxoCmsPageUrlPaths);
    //
    //         var mapped = _pageUrlPathMapper.Map(kx13CmsPageUrlPath, kxoCmsPageUrlPaths);
    //         _migrationProtocol.MappedTarget(mapped);
    //         mapped.LogResult(_logger);
    //
    //         switch (mapped)
    //         {
    //             case ModelMappingSuccess<KXO.Models.CmsPageUrlPath>(var cmsPageUrlPath, var newInstance):
    //                 ArgumentNullException.ThrowIfNull(cmsPageUrlPath, nameof(cmsPageUrlPath));
    //
    //                 if (newInstance)
    //                 {
    //                     _kxoContext.CmsPageUrlPaths.Add(cmsPageUrlPath);
    //                 }
    //                 else
    //                 {
    //                     _kxoContext.CmsPageUrlPaths.Update(cmsPageUrlPath);
    //                 }
    //
    //                 try
    //                 {
    //                     await _kxoContext.SaveChangesAsync(cancellationToken);
    //
    //                     _migrationProtocol.Success(kx13CmsPageUrlPath, cmsPageUrlPath, mapped);
    //                     _logger.LogInformation(newInstance
    //                         ? $"CmsPageUrlPath: {cmsPageUrlPath.PageUrlPathGuid} was inserted."
    //                         : $"CmsPageUrlPath: {cmsPageUrlPath.PageUrlPathGuid} was updated.");
    //                 }
    //                 catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
    //                 {
    //                     throw;
    //                 }
    //
    //                 _primaryKeyMappingContext.SetMapping<KX13.Models.CmsPageUrlPath>(
    //                     r => r.PageUrlPathId,
    //                     kx13CmsPageUrlPath.PageUrlPathId,
    //                     cmsPageUrlPath.PageUrlPathId
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