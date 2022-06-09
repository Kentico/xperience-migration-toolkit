using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Mappers;

public class CmsMediaLibraryMapper: IEntityMapper<KX13.Models.MediaLibrary, KXO.Models.MediaLibrary>
{
    private readonly ILogger<CmsMediaLibraryMapper> _logger;
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public CmsMediaLibraryMapper(
        ILogger<CmsMediaLibraryMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext
        )
    {
        _logger = logger;
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public IModelMappingResult<KXO.Models.MediaLibrary> Map(KX13.Models.MediaLibrary? source, KXO.Models.MediaLibrary? target)
    {
        if (source is null)
        {
            _logger.LogTrace("Source entity is not defined.");
            return new ModelMappingFailedSourceNotDefined<KXO.Models.MediaLibrary>().Log(_logger);
        }

        var newInstance = false;
        if (target is null)
        {
            _logger.LogTrace("Null target supplied, creating new instance.");
            target = new KXO.Models.MediaLibrary();
            newInstance = true;
        }
        else if (source.LibraryGuid != target.LibraryGuid)
        {
            // assertion failed
            _logger.LogTrace("Assertion failed, entity key mismatch.");
            return new ModelMappingFailedKeyMismatch<KXO.Models.MediaLibrary>().Log(_logger);
        }

        // target.LibraryId = source.LibraryId;
        target.LibraryName = source.LibraryName;
        target.LibraryDisplayName = source.LibraryDisplayName;
        target.LibraryDescription = source.LibraryDescription;
        target.LibraryFolder = source.LibraryFolder;
        target.LibraryAccess = source.LibraryAccess;
        target.LibraryGuid = source.LibraryGuid;
        target.LibraryLastModified = source.LibraryLastModified;
        target.LibraryTeaserPath = source.LibraryTeaserPath;
        target.LibraryTeaserGuid = source.LibraryTeaserGuid;
        target.LibraryUseDirectPathForContent = source.LibraryUseDirectPathForContent;
        
        target.LibrarySiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.LibrarySiteId);
        
        // [ForeignKey("LibrarySiteId")]
        // [InverseProperty("MediaLibraries")]
        // public virtual CmsSite LibrarySite { get; set; } = null!;
        // [InverseProperty("FileLibrary")]
        // public virtual ICollection<MediaFile> MediaFiles { get; set; }
        // [InverseProperty("Library")]
        // public virtual ICollection<MediaLibraryRolePermission> MediaLibraryRolePermissions { get; set; }

        return new ModelMappingSuccess<KXO.Models.MediaLibrary>(target, newInstance).Log(_logger);
    }
}