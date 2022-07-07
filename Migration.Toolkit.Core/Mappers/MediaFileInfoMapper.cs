using CMS.Base;
using CMS.DataEngine;
using CMS.MediaLibrary;
using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.Helpers;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Mappers;

public record MediaFileInfoMapperSource(MediaFile MediaFile, int TargetLibraryId, IUploadedFile? File, string? LibrarySubFolder,
    bool MigrateOnlyMediaFileInfo);

public class MediaFileInfoMapper: EntityMapperBase<MediaFileInfoMapperSource, MediaFileInfo>
{
    private readonly IMigrationProtocol _protocol;

    public MediaFileInfoMapper(
        ILogger<MediaFileInfoMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
        ): base(logger, primaryKeyMappingContext, protocol)
    {
        _protocol = protocol;
    }


    protected override MediaFileInfo? CreateNewInstance(MediaFileInfoMapperSource source, MappingHelper mappingHelper, AddFailure addFailure) {
        if (mappingHelper.TranslateRequiredId<KX13M.CmsSite>(s => s.SiteId, source.MediaFile.FileSiteId, out var siteId))
        {
            if (source.File != null)
            {
                return new MediaFileInfo(source.File, source.TargetLibraryId, source.LibrarySubFolder ?? "", 0, 0, 0, siteId);    
            }

            return new MediaFileInfo();
        }

        var error = HandbookReferences
            .FailedToCreateTargetInstance<MediaFileInfo>()
            .WithData(source);
            
        addFailure(new MapperResultFailure<MediaFileInfo>(error));
        return null;
    }

    protected override MediaFileInfo MapInternal(MediaFileInfoMapperSource args, MediaFileInfo target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.FileGuid != target.FileGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<KXO.Models.MediaFile>().Log(_logger);
        // }
        
        var (mediaFile, targetLibraryId, file, _, migrateOnlyMediaFileInfo) = args;
        
        // target.FileId = source.FileId;
        target.FileName = mediaFile.FileName;
        target.FileTitle = mediaFile.FileTitle;
        target.FileDescription = mediaFile.FileDescription;
        target.FileExtension = mediaFile.FileExtension;
        target.FileMimeType = mediaFile.FileMimeType;
        target.FileSize = mediaFile.FileSize;
        target.FileImageWidth = mediaFile.FileImageWidth ?? 0;
        target.FileImageHeight = mediaFile.FileImageHeight ?? 0;
        target.FileGUID = mediaFile.FileGuid;
        target.FileCreatedWhen = mediaFile.FileCreatedWhen;
        target.FileModifiedWhen = mediaFile.FileModifiedWhen;
        KenticoHelper.CopyCustomData(target.FileCustomData, mediaFile.FileCustomData);

        target.FileLibraryID = targetLibraryId;
        
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, mediaFile.FileSiteId, out var siteId))
        {
            target.FileSiteID = siteId;
        }
        
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, mediaFile.FileCreatedByUserId, out var createdByUserId))
        {
            target.SetValue(nameof(target.FileCreatedByUserID), createdByUserId);
        }
        
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, mediaFile.FileModifiedByUserId, out var modifiedByUserId))
        {
            target.SetValue(nameof(target.FileModifiedByUserID), modifiedByUserId);
        }
        
        if (string.IsNullOrWhiteSpace(target.FilePath))
        {
            target.FilePath = mediaFile.FilePath;    
        }

        if (file == null && !migrateOnlyMediaFileInfo)
        {
            _protocol.Append(HandbookReferences.MediaFileIsMissingOnSourceFilesystem
                .WithId(nameof(mediaFile.FileId), mediaFile.FileId)
                .WithData(new
                {
                    mediaFile.FilePath,
                    mediaFile.FileGuid,
                    mediaFile.FileLibraryId,
                    mediaFile.FileSiteId
                })
            );
        }

        return target;
    }
}