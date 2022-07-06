using Microsoft.Extensions.Logging;
using Migration.Toolkit.Core.Abstractions;
using Migration.Toolkit.Core.Contexts;
using Migration.Toolkit.Core.MigrationProtocol;
using Migration.Toolkit.KXO.Models;

namespace Migration.Toolkit.Core.Mappers;

public class CmsMediaLibraryMapper : EntityMapperBase<KX13.Models.MediaLibrary, KXO.Models.MediaLibrary>
{
    public CmsMediaLibraryMapper(
        ILogger<CmsMediaLibraryMapper> logger,
        PrimaryKeyMappingContext primaryKeyMappingContext,
        IMigrationProtocol protocol
    ) : base(logger, primaryKeyMappingContext, protocol)
    {
    }

    protected override MediaLibrary? CreateNewInstance(KX13.Models.MediaLibrary source, MappingHelper mappingHelper, AddFailure addFailure) => new();

    protected override MediaLibrary MapInternal(KX13.Models.MediaLibrary source, MediaLibrary target, bool newInstance, MappingHelper mappingHelper, AddFailure addFailure)
    {
        // if (source.LibraryGuid != target.LibraryGuid)
        // {
        //     // assertion failed
        //     _logger.LogTrace("Assertion failed, entity key mismatch.");
        //     return new ModelMappingFailedKeyMismatch<KXO.Models.MediaLibrary>().Log(_logger);
        // }

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

        // target.LibrarySiteId = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(c => c.SiteId, source.LibrarySiteId);
        if (mappingHelper.TranslateRequiredId<KX13.Models.CmsSite>(c => c.SiteId, source.LibrarySiteId, out var siteId))
        {
            target.LibrarySiteId = siteId;
        }

        return target;
    }
}