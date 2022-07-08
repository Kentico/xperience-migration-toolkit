using System.Diagnostics;
using CMS.Base;
using CMS.DataEngine;
using CMS.MediaLibrary;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Mappers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Context;
using Migration.Toolkit.KX13.Models;
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Api.Auxiliary;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateMediaLibrariesCommandHandler : IRequestHandler<MigrateMediaLibrariesCommand, GenericCommandResult>, IDisposable
{
    private readonly ILogger<MigrateMediaLibrariesCommandHandler> _logger;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly IEntityMapper<MediaLibrary, MediaLibraryInfo> _mediaLibraryInfoMapper;
    private readonly KxoMediaFileFacade _mediaFileFacade;
    private readonly IEntityMapper<MediaFileInfoMapperSource, MediaFileInfo> _mediaFileInfoMapper;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    private KxoContext _kxoContext;

    public MigrateMediaLibrariesCommandHandler(
        ILogger<MigrateMediaLibrariesCommandHandler> logger,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        IEntityMapper<KX13.Models.MediaLibrary, MediaLibraryInfo> mediaLibraryInfoMapper,
        KxoMediaFileFacade mediaFileFacade,
        IEntityMapper<MediaFileInfoMapperSource, MediaFileInfo> mediaFileInfoMapper, 
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kxoContextFactory = kxoContextFactory;
        _kx13ContextFactory = kx13ContextFactory;
        _mediaLibraryInfoMapper = mediaLibraryInfoMapper;
        _mediaFileFacade = mediaFileFacade;
        _mediaFileInfoMapper = mediaFileInfoMapper;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
        _kxoContext = kxoContextFactory.CreateDbContext();
    }

    public async Task<GenericCommandResult> Handle(MigrateMediaLibrariesCommand request, CancellationToken cancellationToken)
    {
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();

        var kx13MediaLibraries = kx13Context.MediaLibraries
                .Include(ml => ml.LibrarySite)
                .Where(x => migratedSiteIds.Contains(x.LibrarySiteId))
                .OrderBy(t => t.LibraryId)
            ;

        var migratedMediaLibraries = new List<(KX13M.MediaLibrary sourceLibrary, MediaLibraryInfo targetLibrary)>();
        foreach (var kx13MediaLibrary in kx13MediaLibraries)
        {
            _migrationProtocol.FetchedSource(kx13MediaLibrary);

            if (!(kx13MediaLibrary.LibraryGuid is Guid mediaLibraryGuid))
            {
                _migrationProtocol.Append(HandbookReferences.FaultyData<KX13M.MediaLibrary>()
                    .WithId(nameof(KX13M.MediaLibrary.LibraryId), kx13MediaLibrary.LibraryId)
                    .WithMessage($"Media library has missing MediaLibraryGUID")
                );
                continue;
            }

            var mediaLibraryInfo = _mediaFileFacade.GetMediaLibraryInfo(mediaLibraryGuid);

            _migrationProtocol.FetchedTarget(mediaLibraryInfo);

            var mapped = _mediaLibraryInfoMapper.Map(kx13MediaLibrary, mediaLibraryInfo);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is { Success : true } result)
            {
                var (mfi, newInstance) = result;
                ArgumentNullException.ThrowIfNull(mfi, nameof(mfi));

                try
                {
                    _mediaFileFacade.SetMediaLibrary(mfi);

                    _migrationProtocol.Success(kx13MediaLibrary, mfi, mapped);
                    _logger.LogEntitySetAction(newInstance, mfi);
                }
                catch (Exception ex)
                {
                    await _kxoContext.DisposeAsync(); // reset context errors
                    _kxoContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                    _migrationProtocol.Append(HandbookReferences
                        .ErrorCreatingTargetInstance<MediaLibraryInfo>(ex)
                        .NeedsManualAction()
                        .WithIdentityPrint(mfi)
                    );
                    _logger.LogEntitySetError(ex, newInstance, mfi);
                    continue;
                }

                _primaryKeyMappingContext.SetMapping<KX13.Models.MediaLibrary>(
                    r => r.LibraryId,
                    kx13MediaLibrary.LibraryId,
                    mfi.LibraryID
                );

                migratedMediaLibraries.Add((kx13MediaLibrary, mfi));
            }
        }

        await RequireMigratedMediaFiles(migratedSiteIds, migratedMediaLibraries, kx13Context, cancellationToken);

        return new GenericCommandResult();
    }

    private record LoadMediaFileResult(bool Found, IUploadedFile? File);
    private LoadMediaFileResult LoadMediaFileBinary(string? sourceMediaLibraryPath, string relativeFilePath, string contentType)
    {
        if (sourceMediaLibraryPath == null)
        {
            return new LoadMediaFileResult(false, null);
        }

        var filePath = Path.Combine(sourceMediaLibraryPath, relativeFilePath);
        if (File.Exists(filePath))
        {
            var data = File.ReadAllBytes(filePath);
            var dummyFile = DummyUploadedFile.FromByteArray(data, contentType, data.LongLength, Path.GetFileName(filePath));
            return new LoadMediaFileResult(true, dummyFile);
        }

        return new LoadMediaFileResult(false, null);
    }

    private async Task RequireMigratedMediaFiles(List<int?> migratedSiteIds,
        List<(MediaLibrary sourceLibrary, MediaLibraryInfo targetLibrary)> migratedMediaLibraries,
        KX13Context kx13Context, CancellationToken cancellationToken)
    {
        var kxoDbContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);
        try
        {
            foreach (var (sourceMediaLibrary, targetMediaLibrary) in migratedMediaLibraries)
            {
                string? sourceMediaLibraryPath = null;
                var loadMediaFileData = false;
                if (!_toolkitConfiguration.MigrateOnlyMediaFileInfo.GetValueOrDefault(true) &&
                    !string.IsNullOrWhiteSpace(_toolkitConfiguration.SourceCmsDirPath))
                {
                    sourceMediaLibraryPath = Path.Join(_toolkitConfiguration.SourceCmsDirPath, sourceMediaLibrary.LibrarySite.SiteName, "media",
                        sourceMediaLibrary.LibraryFolder);
                    loadMediaFileData = true;
                }

                var targetSite = kxoDbContext.CmsSites.SingleOrDefault(s => s.SiteId == targetMediaLibrary.LibrarySiteID);
                var kx13MediaFiles = kx13Context.MediaFiles
                    .Where(x => migratedSiteIds.Contains(x.FileSiteId))
                    .Where(x => x.FileLibraryId == sourceMediaLibrary.LibraryId);

                foreach (var kx13MediaFile in kx13MediaFiles)
                {
                    _migrationProtocol.FetchedSource(kx13MediaFile);

                    bool found = false;
                    IUploadedFile? uploadedFile = null;
                    if (loadMediaFileData)
                    {
                        (found, uploadedFile) = LoadMediaFileBinary(sourceMediaLibraryPath, kx13MediaFile.FilePath, kx13MediaFile.FileMimeType);
                        if (!found)
                        {
                            // TODO tk: 2022-07-07 report missing file (currently reported in mapper)
                        }
                    }

                    var librarySubfolder = Path.GetDirectoryName(kx13MediaFile.FilePath);

                    var kxoMediaFile = _mediaFileFacade.GetMediaFile(kx13MediaFile.FileGuid);

                    _migrationProtocol.FetchedTarget(kxoMediaFile);

                    var source = new MediaFileInfoMapperSource(kx13MediaFile, targetMediaLibrary.LibraryID, found ? uploadedFile : null,
                        librarySubfolder, _toolkitConfiguration.MigrateOnlyMediaFileInfo.GetValueOrDefault(false));
                    var mapped = _mediaFileInfoMapper.Map(source, kxoMediaFile);
                    _migrationProtocol.MappedTarget(mapped);

                    switch (mapped)
                    {
                        case { Success : true } result:
                        {
                            var (mf, newInstance) = result;
                            ArgumentNullException.ThrowIfNull(mf, nameof(mf));

                            try
                            {
                                if (newInstance)
                                {
                                    Debug.Assert(targetSite != null, nameof(targetSite) + " != null");
                                    _mediaFileFacade.EnsureMediaFilePathExistsInLibrary(mf, targetMediaLibrary.LibraryID, targetSite.SiteName);
                                }

                                _mediaFileFacade.SetMediaFile(mf, newInstance);
                                await _kxoContext.SaveChangesAsync(cancellationToken);

                                _migrationProtocol.Success(kx13MediaFile, mf, mapped);
                                _logger.LogEntitySetAction(newInstance, mf);
                            }
                            catch (Exception ex) // TODO tk: 2022-05-18 handle exceptions
                            {
                                await kxoDbContext.DisposeAsync(); // reset context errors
                                kxoDbContext = await _kxoContextFactory.CreateDbContextAsync(cancellationToken);

                                _migrationProtocol.Append(HandbookReferences
                                    .ErrorCreatingTargetInstance<MediaLibraryInfo>(ex)
                                    .NeedsManualAction()
                                    .WithIdentityPrint(mf)
                                );
                                _logger.LogEntitySetError(ex, newInstance, mf);
                                continue;
                            }

                            _primaryKeyMappingContext.SetMapping<KX13.Models.MediaFile>(
                                r => r.FileId,
                                kx13MediaFile.FileId,
                                mf.FileID
                            );

                            break;
                        }
                    }
                }
            }
        }
        finally
        {
            kxoDbContext.Dispose();
        }
    }

    public void Dispose()
    {
        _kxoContext.Dispose();
    }
}