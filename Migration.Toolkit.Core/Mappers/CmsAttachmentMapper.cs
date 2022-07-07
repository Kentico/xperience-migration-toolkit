using CMS.Base;
using CMS.MediaLibrary;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Helpers;
using Migration.Toolkit.Core.MigrationProtocol;

namespace Migration.Toolkit.Core.Mappers;

public record CmsAttachmentMapperSource(KX13M.CmsAttachment Attachment, int TargetLibraryId, IUploadedFile File, string LibrarySubFolder);

public class CmsAttachmentMapper: EntityMapperBase<CmsAttachmentMapperSource, MediaFileInfo>
{
    public CmsAttachmentMapper(ILogger<CmsAttachmentMapper> logger, PrimaryKeyMappingContext pkContext, IMigrationProtocol protocol) : base(logger, pkContext, protocol)
    {
    }

    protected override MediaFileInfo? CreateNewInstance(CmsAttachmentMapperSource source, MappingHelper mappingHelper, AddFailure addFailure)
    {
        if (mappingHelper.TranslateRequiredId<KX13M.CmsSite>(s => s.SiteId, source.Attachment.AttachmentSiteId, out var siteId))
        {
            return new MediaFileInfo(source.File, source.TargetLibraryId, source.LibrarySubFolder, 0, 0, 0, siteId);
        }

        var error = HandbookReferences
            .FailedToCreateTargetInstance<MediaFileInfo>()
            .WithData(source);
            
        addFailure(new MapperResultFailure<MediaFileInfo>(error));
        return null;
    }

    protected override MediaFileInfo MapInternal(CmsAttachmentMapperSource args, MediaFileInfo target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // target.FileId = source.FileId;
        var (cmsAttachment, targetLibraryId, _, _) = args;
        
        target.FileName = cmsAttachment.AttachmentName;
        target.FileTitle = cmsAttachment.AttachmentTitle ?? cmsAttachment.AttachmentName;
        target.FileDescription = cmsAttachment.AttachmentDescription ?? string.Empty;
        target.FileExtension = cmsAttachment.AttachmentExtension;
        target.FileMimeType = cmsAttachment.AttachmentMimeType;
        // target.FilePath = targetMediaFilePath;
        target.FileSize = cmsAttachment.AttachmentSize;
        target.FileImageWidth = cmsAttachment.AttachmentImageWidth ?? 0;
        target.FileImageHeight = cmsAttachment.AttachmentImageHeight ?? 0;
        target.FileGUID = cmsAttachment.AttachmentGuid;
        target.FileLibraryID = targetLibraryId;
        // target.FileSiteID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(s => s.SiteId, source.AttachmentSiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(s => s.SiteId, cmsAttachment.AttachmentSiteId, out var siteId))
        {
            target.FileSiteID = siteId;
        }

        // TODO tk: 2022-06-29 user not migrated
        // target.FileCreatedByUserID = source.;
        // target.FileModifiedByUserID = source.FileModifiedByUserId;

        // TODO tk: 2022-06-29 temporal info not migrated
        // target.FileCreatedWhen = source.;
        // target.FileModifiedWhen = source.FileModifiedWhen;

        KenticoHelper.CopyCustomData(target.FileCustomData, cmsAttachment.AttachmentCustomData);
        
        return target;
    }
}