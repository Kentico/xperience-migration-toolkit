namespace Migration.Toolkit.Core.KX12.Services;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CMS.Base;
using CMS.Helpers;
using CMS.MediaLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Common;
using Migration.Toolkit.Common.Abstractions;
using Migration.Toolkit.Common.MigrationProtocol;
using Migration.Toolkit.Core.KX12.Contexts;
using Migration.Toolkit.Core.KX12.Mappers;
using Migration.Toolkit.KX12.Context;
using Migration.Toolkit.KXP.Api;
using Migration.Toolkit.KXP.Api.Auxiliary;
using Migration.Toolkit.KXP.Context;

public class AttachmentMigrator
{
    private readonly ILogger<AttachmentMigrator> _logger;
    private readonly IDbContextFactory<KX12Context> _kx12ContextFactory;
    private readonly KxpMediaFileFacade _mediaFileFacade;
    private readonly IDbContextFactory<KxpContext> _kxpContextFactory;
    private readonly IEntityMapper<CmsAttachmentMapperSource, MediaFileInfo> _attachmentMapper;
    private readonly IProtocol _protocol;

    public AttachmentMigrator(
        ILogger<AttachmentMigrator> logger,
        IDbContextFactory<KX12Context> kx12ContextFactory,
        KxpMediaFileFacade mediaFileFacade,
        IDbContextFactory<KxpContext> kxpContextFactory,
        IEntityMapper<CmsAttachmentMapperSource, MediaFileInfo> attachmentMapper,
        IProtocol protocol
    )
    {
        _logger = logger;
        _kx12ContextFactory = kx12ContextFactory;
        _mediaFileFacade = mediaFileFacade;
        _kxpContextFactory = kxpContextFactory;
        _attachmentMapper = attachmentMapper;
        _protocol = protocol;
    }

    public record MigrateAttachmentResult(bool Success, bool CanContinue, MediaFileInfo? MediaFileInfo = null,
        MediaLibraryInfo? MediaLibraryInfo = null);

    public MigrateAttachmentResult TryMigrateAttachmentByPath(string documentPath, string additionalPath)
    {
        if(string.IsNullOrWhiteSpace(documentPath)) return new MigrateAttachmentResult(false, false);
        documentPath = $"/{documentPath.Trim('/')}";

        using var kx12Context = _kx12ContextFactory.CreateDbContext();
        var attachment =
            kx12Context.CmsAttachments
                .Include(a => a.AttachmentDocument)
                .ThenInclude(d => d.DocumentNode)
                .FirstOrDefault(a => a.AttachmentDocument.DocumentNode.NodeAliasPath == documentPath);

        return attachment != null
            ? MigrateAttachment(attachment, additionalPath)
            : new MigrateAttachmentResult(false, false);
    }

    public IEnumerable<MigrateAttachmentResult> MigrateGroupedAttachments(int documentId, Guid attachmentGroupGuid, string fieldName)
    {
        using var kx12Context = _kx12ContextFactory.CreateDbContext();
        var groupedAttachment =
            kx12Context.CmsAttachments.Where(a => a.AttachmentGroupGuid == attachmentGroupGuid && a.AttachmentDocumentId == documentId);
        foreach (var cmsAttachment in groupedAttachment)
        {
            yield return MigrateAttachment(cmsAttachment, $"__{fieldName}");
        }
    }

    public MigrateAttachmentResult MigrateAttachment(Guid k12CmsAttachmentGuid, string additionalPath)
    {
        using var kx12Context = _kx12ContextFactory.CreateDbContext();
        var attachment = kx12Context.CmsAttachments.SingleOrDefault(a => a.AttachmentGuid == k12CmsAttachmentGuid);
        if (attachment == null)
        {
            _logger.LogWarning("Attachment '{AttachmentGuid}' not found! => skipping", k12CmsAttachmentGuid);
            _protocol.Append(HandbookReferences.TemporaryAttachmentMigrationIsNotSupported.WithData(new
            {
                AttachmentGuid = k12CmsAttachmentGuid,
            }));
            return new MigrateAttachmentResult(false, true);
        }

        return MigrateAttachment(attachment, additionalPath);
    }

    public MigrateAttachmentResult MigrateAttachment(KX12M.CmsAttachment kx12CmsAttachment, string? additionalMediaPath = null)
    {
        // TODO tomas.krch: 2022-08-18 directory validation only -_ replace!
        _protocol.FetchedSource(kx12CmsAttachment);

        if (kx12CmsAttachment.AttachmentFormGuid != null)
        {
            _logger.LogWarning("Attachment '{AttachmentGuid}' is temporary => skipping", kx12CmsAttachment.AttachmentGuid);
            _protocol.Append(HandbookReferences.TemporaryAttachmentMigrationIsNotSupported.WithData(new
            {
                kx12CmsAttachment.AttachmentId,
                kx12CmsAttachment.AttachmentGuid,
                kx12CmsAttachment.AttachmentName,
                kx12CmsAttachment.AttachmentSiteId
            }));
            return new(false, true);
        }

        var kx12AttachmentDocument = kx12CmsAttachment.AttachmentDocumentId is int attachmentDocumentId
            ? GetK12CmsDocument(attachmentDocumentId)
            : null;

        using var kx12Context = _kx12ContextFactory.CreateDbContext();
        var site = kx12Context.CmsSites.FirstOrDefault(s=>s.SiteId == kx12CmsAttachment.AttachmentSiteId) ?? throw new InvalidOperationException("Site not exists!");
        if (!TryEnsureTargetLibraryExists(kx12CmsAttachment.AttachmentSiteId, site.SiteName, out var targetMediaLibraryId))
        {
            return new(false, false);
        }

        var uploadedFile = CreateUploadFileFromAttachment(kx12CmsAttachment);
        if (uploadedFile == null)
        {
            _protocol.Append(HandbookReferences
                .FailedToCreateTargetInstance<MediaFileInfo>()
                .WithIdentityPrint(kx12CmsAttachment)
                .WithMessage("Failed to create dummy upload file containing data")
            );
            if (kx12CmsAttachment.AttachmentBinary == null)
            {
                _logger.LogWarning("Failed to migrate attachment {Guid}, AttachmentBinary is null", kx12CmsAttachment.AttachmentGuid);
            }
            else
            {
                _logger.LogWarning("Failed to migrate attachment {Guid}", kx12CmsAttachment.AttachmentGuid);
            }

            return new(false, true);
        }

        var mediaFile = _mediaFileFacade.GetMediaFile(kx12CmsAttachment.AttachmentGuid);

        _protocol.FetchedTarget(mediaFile);

        var librarySubFolder = "";
        if (kx12AttachmentDocument != null)
        {
            librarySubFolder = kx12AttachmentDocument.DocumentNode.NodeAliasPath;
        }

        if (!string.IsNullOrWhiteSpace(additionalMediaPath) && (kx12CmsAttachment.AttachmentIsUnsorted != true || kx12CmsAttachment.AttachmentGroupGuid != null))
        {
            librarySubFolder = System.IO.Path.Combine(librarySubFolder, additionalMediaPath);
        }

        var mapped = _attachmentMapper.Map(new CmsAttachmentMapperSource(kx12CmsAttachment, targetMediaLibraryId, uploadedFile, librarySubFolder, kx12AttachmentDocument), mediaFile);
        _protocol.MappedTarget(mapped);

        if (mapped is (var mediaFileInfo, var newInstance) { Success: true })
        {
            Debug.Assert(mediaFileInfo != null, nameof(mediaFileInfo) + " != null");

            try
            {
                if (newInstance)
                {
                    _mediaFileFacade.EnsureMediaFilePathExistsInLibrary(mediaFileInfo, targetMediaLibraryId);
                }

                _mediaFileFacade.SetMediaFile(mediaFileInfo, newInstance);

                _protocol.Success(kx12AttachmentDocument, mediaFileInfo, mapped);
                _logger.LogEntitySetAction(newInstance, mediaFileInfo);

                return new(true, true, mediaFileInfo, MediaLibraryInfoProvider.ProviderObject.Get(targetMediaLibraryId));
            }
            catch (Exception exception)
            {
                _logger.LogEntitySetError(exception, newInstance, mediaFileInfo);
                _protocol.Append(HandbookReferences.ErrorCreatingTargetInstance<MediaFileInfo>(exception)
                    .NeedsManualAction()
                    .WithIdentityPrint(mediaFileInfo)
                    .WithData(new
                    {
                        mediaFileInfo.FileGUID,
                        mediaFileInfo.FileName,
                        // TODOV27 tomas.krch: 2023-09-05: obsolete - fileSiteID
                        // mediaFileInfo.FileSiteID
                    })
                );
            }
        }

        return new(false, true);
    }

    private KX12M.CmsDocument? GetK12CmsDocument(int documentId)
    {
        using var dbContext = _kx12ContextFactory.CreateDbContext();
        return dbContext.CmsDocuments
            .Include(d => d.DocumentNode)
            .SingleOrDefault(a => a.DocumentId == documentId);
    }


    private IUploadedFile? CreateUploadFileFromAttachment(KX12M.CmsAttachment attachment)
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

    private bool TryEnsureTargetLibraryExists(int targetSiteId, string targetSiteName, out int targetLibraryId)
    {
        var targetLibraryCodeName = $"AttachmentsForSite{targetSiteName}";
        var targetLibraryDisplayName = $"Attachments for site {targetSiteName}";
        using var dbContext = _kxpContextFactory.CreateDbContext();
        try
        {
            targetLibraryId = _mediaLibraryIdCache.GetOrAdd((targetLibraryCodeName, targetSiteId), static (arg, context) => MediaLibraryFactory(arg, context), new MediaLibraryFactoryContext(_mediaFileFacade, targetLibraryCodeName, targetLibraryDisplayName, dbContext));

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "creating target media library failed");
            _protocol.Append(HandbookReferences.ErrorCreatingTargetInstance<MediaLibraryInfo>(exception)
                .NeedsManualAction()
                .WithData(new
                {
                    TargetLibraryCodeName = targetLibraryCodeName,
                    targetSiteId,
                })
            );
        }

        targetLibraryId = 0;
        return false;
    }

    private record MediaLibraryFactoryContext(KxpMediaFileFacade MediaFileFacade, string TargetLibraryCodeName, string TargetLibraryDisplayName, KxpContext DbContext);

    private static readonly Regex SanitizationRegex =
        RegexHelper.GetRegex("[^-_a-z0-9]", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex LibraryPathValidationRegex =
        RegexHelper.GetRegex("^[-_a-z0-9]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

    private static int MediaLibraryFactory((string libraryName, int siteId) arg, MediaLibraryFactoryContext context)
    {
        var (libraryName, siteId) = arg;

        // TODO tomas.krch: 2023-11-02 libraries now globalized, where do i put conflicting directories?
        var tml = context.DbContext.MediaLibraries.SingleOrDefault(ml => ml.LibraryName == libraryName);

        var libraryDirectory = context.TargetLibraryCodeName;
        if (!LibraryPathValidationRegex.IsMatch(libraryDirectory))
        {
            libraryDirectory = SanitizationRegex.Replace(libraryDirectory, "_");
        }

        return tml?.LibraryId ?? context.MediaFileFacade.CreateMediaLibrary(siteId, libraryDirectory, "Created by Xperience Migration.Toolkit", context.TargetLibraryCodeName, context.TargetLibraryDisplayName).LibraryID;
    }
}