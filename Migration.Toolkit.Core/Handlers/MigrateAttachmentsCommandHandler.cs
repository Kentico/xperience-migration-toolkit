using System.Collections.Concurrent;
using System.Diagnostics;
using CMS.Base;
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
using Migration.Toolkit.KXO.Api;
using Migration.Toolkit.KXO.Api.Auxiliary;
using Migration.Toolkit.KXO.Context;

namespace Migration.Toolkit.Core.Handlers;

public class MigrateAttachmentsCommandHandler : IRequestHandler<MigrateAttachmentsCommand, CommandResult>, IDisposable
{
    private readonly ILogger<MigrateAttachmentsCommandHandler> _logger;
    private readonly IDbContextFactory<KX13Context> _kx13ContextFactory;
    private readonly ToolkitConfiguration _toolkitConfiguration;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly KxoMediaFileFacade _mediaFileFacade;
    private readonly IDbContextFactory<KxoContext> _kxoContextFactory;

    private readonly IEntityMapper<CmsAttachmentMapperSource, MediaFileInfo> _attachmentMapper;
    private readonly IMigrationProtocol _migrationProtocol;

    public MigrateAttachmentsCommandHandler(
        ILogger<MigrateAttachmentsCommandHandler> logger,
        IDbContextFactory<KX13.Context.KX13Context> kx13ContextFactory,
        ToolkitConfiguration toolkitConfiguration,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        KxoMediaFileFacade mediaFileFacade,
        IDbContextFactory<KXO.Context.KxoContext> kxoContextFactory,
        IEntityMapper<CmsAttachmentMapperSource, MediaFileInfo> attachmentMapper,
        IMigrationProtocol migrationProtocol
    )
    {
        _logger = logger;
        _kx13ContextFactory = kx13ContextFactory;
        _toolkitConfiguration = toolkitConfiguration;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _mediaFileFacade = mediaFileFacade;
        _kxoContextFactory = kxoContextFactory;
        _attachmentMapper = attachmentMapper;
        _migrationProtocol = migrationProtocol;
    }
    
    public async Task<CommandResult> Handle(MigrateAttachmentsCommand request, CancellationToken cancellationToken)
    {
        var migratedSiteIds = _toolkitConfiguration.RequireExplicitMapping<KX13.Models.CmsSite>(s => s.SiteId).Keys.ToList();
        await using var kx13Context = await _kx13ContextFactory.CreateDbContextAsync(cancellationToken);

        var kx13CmsAttachments = kx13Context.CmsAttachments
            .Where(x => migratedSiteIds.Contains(x.AttachmentSiteId));

        var libraryNameMask = _toolkitConfiguration.TargetAttachmentMediaLibraryName;
        if (string.IsNullOrWhiteSpace(libraryNameMask))
        {
            _migrationProtocol.Append(HandbookReferences
                .MissingConfiguration<MigrateAttachmentsCommand>(nameof(_toolkitConfiguration.TargetAttachmentMediaLibraryName))
            );
            return new CommandFailureResult();
        }
        
        var targetSites = new ConcurrentDictionary<int, KXOM.CmsSite>();
        
        foreach (var kx13CmsAttachment in kx13CmsAttachments)
        {
            _migrationProtocol.FetchedSource(kx13CmsAttachment);
            
            if (kx13CmsAttachment.AttachmentFormGuid != null)
            {
                _logger.LogWarning("Attachment '{AttachmentGuid}' is temporary => skipping", kx13CmsAttachment.AttachmentGuid);
                _migrationProtocol.Append(HandbookReferences.TemporaryAttachmentMigrationIsNotSupported.WithData(new
                {
                    kx13CmsAttachment.AttachmentId,
                    kx13CmsAttachment.AttachmentGuid,
                    kx13CmsAttachment.AttachmentName,
                    kx13CmsAttachment.AttachmentSiteId
                }));
                continue;
            }

            var kx13AttachmentDocument = kx13CmsAttachment.AttachmentDocumentId is int attachmentDocumentId ? await GetKx13CmsDocument(attachmentDocumentId) : null;
            var targetSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(s => s.SiteId, kx13CmsAttachment.AttachmentSiteId);
            var targetSite = targetSites.GetOrAdd(targetSiteId, i =>
            {
                using var kxoDbContext = _kxoContextFactory.CreateDbContext();
                return kxoDbContext.CmsSites.Single(s => s.SiteId == targetSiteId);
            });

            var targetLibraryName = libraryNameMask
                    .Replace("{sitename}", targetSite.SiteName, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("{siteid}", targetSite.SiteId.ToString(), StringComparison.InvariantCultureIgnoreCase)
                ;

            if (!TryEnsureTargetLibraryExists(targetLibraryName, targetSite.SiteId, out var targetMediaLibraryId))
            {
                break;
            }
            
            var uploadedFile = CreateUploadFileFromAttachment(kx13CmsAttachment);
            if (uploadedFile == null)
            {
                _migrationProtocol.Append(HandbookReferences
                    .FailedToCreateTargetInstance<MediaFileInfo>()
                    .WithIdentityPrint(kx13CmsAttachment)
                    .WithMessage("Failed to create dummy upload file containing data")
                );
                continue;
            }
            
            var mediaFile = _mediaFileFacade.GetMediaFile(kx13CmsAttachment.AttachmentGuid);
            
            _migrationProtocol.FetchedTarget(mediaFile);

            var librarySubFolder = "";
            if (kx13AttachmentDocument != null)
            {
                librarySubFolder = kx13AttachmentDocument.DocumentNode.NodeAliasPath;
            }

            var mapped = _attachmentMapper.Map(new CmsAttachmentMapperSource(kx13CmsAttachment, targetMediaLibraryId, uploadedFile, librarySubFolder), mediaFile);
            _migrationProtocol.MappedTarget(mapped);

            if (mapped is (var mediaFileInfo, var newInstance) { Success: true })
            {
                Debug.Assert(mediaFileInfo != null, nameof(mediaFileInfo) + " != null");

                try
                {
                    if (newInstance)
                    {
                        _mediaFileFacade.EnsureMediaFilePathExistsInLibrary(mediaFileInfo, targetMediaLibraryId, targetSite.SiteName);
                    }

                    _mediaFileFacade.SetMediaFile(mediaFileInfo, newInstance);

                    _migrationProtocol.Success(kx13AttachmentDocument, mediaFileInfo, mapped);
                    _logger.LogEntitySetAction(newInstance, mediaFileInfo);
                }
                catch (Exception exception)
                {
                    _logger.LogEntitySetError(exception, newInstance, mediaFileInfo);
                    _migrationProtocol.Append(HandbookReferences.ErrorCreatingTargetInstance<MediaFileInfo>(exception)
                        .NeedsManualAction()
                        .WithIdentityPrint(mediaFileInfo)
                        .WithData(new
                        {
                            targetLibraryName,
                            targetSiteId,
                            mediaFileInfo.FileGUID,
                            mediaFileInfo.FileName,
                            mediaFileInfo.FileSiteID
                        })
                    );
                }
            }
        }

        return new GenericCommandResult();
    }
    
     private async Task<KX13M.CmsDocument?> GetKx13CmsDocument(int documentId)
    {
        await using var dbContext = await _kx13ContextFactory.CreateDbContextAsync();
        return await dbContext.CmsDocuments
            .Include(d => d.DocumentNode)
            .Where(a => a.DocumentId == documentId).SingleOrDefaultAsync();
    }
    
    private IUploadedFile? CreateUploadFileFromAttachment(KX13M.CmsAttachment attachment)
    {
        if (attachment.AttachmentBinary != null)
        {
            var ms = new MemoryStream(attachment.AttachmentBinary);
            return DummyUploadedFile.FromStream(ms, attachment.AttachmentMimeType, attachment.AttachmentSize, attachment.AttachmentName);
        }
        else
        {
            return null;
        }
    }

    private readonly ConcurrentDictionary<(string libraryName, int siteId), int> _mediaLibraryIdCache = new();
    private int EnsureMediaFileLibrary((string libraryName, int siteId) arg, KxoContext db)
    {
        var (libraryName, siteId) = arg;
        var tml = db.MediaLibraries.SingleOrDefault(ml => ml.LibrarySiteId == siteId && ml.LibraryName == libraryName);
        return tml?.LibraryId ?? _mediaFileFacade.CreateMediaLibrary(siteId, libraryName, "", libraryName, libraryName).LibraryID;
    }
    private bool TryEnsureTargetLibraryExists(string targetLibraryName, int targetSiteId, out int targetLibraryId)
    {
        using var dbContext = _kxoContextFactory.CreateDbContext();
        try
        {
            targetLibraryId = _mediaLibraryIdCache.GetOrAdd((targetLibraryName, targetSiteId), EnsureMediaFileLibrary, dbContext);
            
            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "creating target media library failed");
            _migrationProtocol.Append(HandbookReferences.ErrorCreatingTargetInstance<MediaLibraryInfo>(exception)
                .NeedsManualAction()
                .WithData(new
                {
                    targetLibraryName,
                    targetSiteId,
                })
            );
        }

        targetLibraryId = 0;
        return false;
    }

    public void Dispose()
    {
        
    }
}