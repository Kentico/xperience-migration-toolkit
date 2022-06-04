using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;

namespace Migration.Toolkit.Core.Mappers;

public class CmsMediaFileMapper: IEntityMapper<KX13.Models.MediaFile, KXO.Models.MediaFile>
{
    private readonly ILogger<CmsMediaFileMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;
    private readonly IMigrationProtocol _migrationProtocol;

    public CmsMediaFileMapper(
        ILogger<CmsMediaFileMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol migrationProtocol
        )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
        _migrationProtocol = migrationProtocol;
    }
    
    public IModelMappingResult<KXO.Models.MediaFile> Map(KX13.Models.MediaFile? source, KXO.Models.MediaFile? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.MediaFile>();
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new KXO.Models.MediaFile();
            newInstance = true;
        }
        else if (source.FileGuid != target.FileGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<KXO.Models.MediaFile>();
        }

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
        target.FileSiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.FileSiteId);
        target.FileCreatedByUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.FileCreatedByUserId);
        target.FileModifiedByUserId = _primaryKeyMappingContext.MapFromSource<KX13.Models.CmsUser>(c => c.UserId, source.FileModifiedByUserId);

        // TODO tk: 2022-05-20 foreign binary dep => ref to handbook
        target.FilePath = source.FilePath;
        
        _migrationProtocol.NeedsManualAction(HandbookReferences.MediaFileMigrateFileManually, "Document must be migrated manually", source, target);
        
        // [ForeignKey("FileCreatedByUserId")]
        // [InverseProperty("MediaFileFileCreatedByUsers")]
        // public virtual CmsUser? FileCreatedByUser { get; set; }
        // [ForeignKey("FileLibraryId")]
        // [InverseProperty("MediaFiles")]
        // public virtual MediaLibrary FileLibrary { get; set; } = null!;
        // [ForeignKey("FileModifiedByUserId")]
        // [InverseProperty("MediaFileFileModifiedByUsers")]
        // public virtual CmsUser? FileModifiedByUser { get; set; }
        // [ForeignKey("FileSiteId")]
        // [InverseProperty("MediaFiles")]
        // public virtual CmsSite FileSite { get; set; } = null!;
        
        //
        // // target.LibraryId = source.LibraryId;
        // target.LibraryName = source.LibraryName;
        // target.LibraryDisplayName = source.LibraryDisplayName;
        // target.LibraryDescription = source.LibraryDescription;
        // target.LibraryFolder = source.LibraryFolder;
        // target.LibraryAccess = source.LibraryAccess;
        // // target.LibraryGuid = source.LibraryGuid;
        // target.LibraryLastModified = source.LibraryLastModified;
        // target.LibraryTeaserPath = source.LibraryTeaserPath;
        // target.LibraryTeaserGuid = source.LibraryTeaserGuid;
        // target.LibraryUseDirectPathForContent = source.LibraryUseDirectPathForContent;
        //
        // target.LibrarySiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.LibrarySiteId);
        //
        // // [ForeignKey("LibrarySiteId")]
        // // [InverseProperty("MediaLibraries")]
        // // public virtual CmsSite LibrarySite { get; set; } = null!;
        // // [InverseProperty("FileLibrary")]
        // // public virtual ICollection<MediaFile> MediaFiles { get; set; }
        // // [InverseProperty("Library")]
        // // public virtual ICollection<MediaLibraryRolePermission> MediaLibraryRolePermissions { get; set; }

        return new ModelMappingSuccess<KXO.Models.MediaFile>(target, newInstance);
    }
}