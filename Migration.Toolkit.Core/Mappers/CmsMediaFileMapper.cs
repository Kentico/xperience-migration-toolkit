using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

[Obsolete("Use variant with kentico info object or finish implementation", true)]
public class CmsMediaFileMapper: EntityMapperBase<KX13.Models.MediaFile, KXO.Models.MediaFile>
{
    private readonly IMigrationProtocol _protocol;

    public CmsMediaFileMapper(
        ILogger<CmsMediaFileMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
        ): base(logger, primaryKeyMappingContext, protocol)
    {
        _protocol = protocol;
    }


    protected override MediaFile? CreateNewInstance(KX13.Models.MediaFile source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override MediaFile MapInternal(KX13.Models.MediaFile source, MediaFile target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.FileGuid != target.FileGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<KXO.Models.MediaFile>().Log(_logger);
        // }

        // target.FileId = source.FileId;
        target.FileName = source.FileName;
        target.FileTitle = source.FileTitle;
        target.FileDescription = source.FileDescription;
        target.FileExtension = source.FileExtension;
        target.FileMimeType = source.FileMimeType;
        target.FileSize = source.FileSize;
        target.FileImageWidth = source.FileImageWidth;
        target.FileImageHeight = source.FileImageHeight;
        target.FileGuid = source.FileGuid;
        target.FileCreatedWhen = source.FileCreatedWhen;
        target.FileModifiedWhen = source.FileModifiedWhen;
        target.FileCustomData = source.FileCustomData;

        target.FileLibraryId = source.FileLibraryId;
        
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.FileSiteId, out var siteId))
        {
            target.FileSiteId = siteId;
        }
        
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, source.FileCreatedByUserId, out var createdByUserId))
        {
            target.FileCreatedByUserId = createdByUserId;
        }
        
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, source.FileModifiedByUserId, out var modifiedByUserId))
        {
            target.FileModifiedByUserId = modifiedByUserId;
        }
        
        target.FilePath = source.FilePath;
        
        _protocol.Append(HandbookReferences.MediaFileIsMissingOnSourceFilesystem
            .WithId(nameof(source.FileId), source.FileId)
            .WithData(new
            {
                source.FilePath,
                source.FileGuid,
                source.FileLibraryId,
                source.FileSiteId
            })
        );

        return target;
    }
}