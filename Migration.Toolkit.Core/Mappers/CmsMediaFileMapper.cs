using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsMediaFileMapper: EntityMapperBase<KX13.Models.MediaFile, KXO.Models.MediaFile>
{
    private readonly ILogger<CmsMediaFileMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _protocol;

    public CmsMediaFileMapper(
        ILogger<CmsMediaFileMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
        ): base(logger, primaryKeyMappingContext, protocol)
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
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
        
        // target.FileSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FileSiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.FileSiteId, out var siteId))
        {
            target.FileSiteId = siteId;
        }
        
        // target.FileCreatedByUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.FileCreatedByUserId);
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, source.FileCreatedByUserId, out var createdByUserId))
        {
            target.FileCreatedByUserId = createdByUserId;
        }
        
        // target.FileModifiedByUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.FileModifiedByUserId);
        if (mappingHelper.TranslateId<KX13.Models.CmsUser>(c => c.UserId, source.FileModifiedByUserId, out var modifiedByUserId))
        {
            target.FileModifiedByUserId = modifiedByUserId;
        }
        
        // TODO tk: 2022-05-20 foreign binary dep => ref to handbook
        target.FilePath = source.FilePath;
        
        _protocol.NeedsManualAction(HandbookReferences.MediaFileMigrateFileManually, "Document must be migrated manually", source, target);

        return target;
    }
}