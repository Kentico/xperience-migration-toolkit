using CMS.MediaLibrary;
using Microsoft.Extensions.Logging;

namespace Migration.Toolkit.KXO.Api;

public class KxoMediaFileFacade
{
    private readonly ILogger<KxoMediaFileFacade> _logger;
    private readonly KxoApiInitializer _kxoApiInitializer;

    public KxoMediaFileFacade(ILogger<KxoMediaFileFacade> logger, KxoApiInitializer kxoApiInitializer)
    {
        _logger = logger;
        _kxoApiInitializer = kxoApiInitializer;
        
        _kxoApiInitializer.EnsureApiIsInitialized();
    }

    public void InsertMediaFile(MediaFileInfo mfi)
    {
        MediaFileInfoProvider.ImportMediaFileInfo(mfi);
    }

    // public MediaLibraryInfo EnsureLibraryExists(int siteId, string mediaLibraryName)
    // {
    //     MediaLibraryInfoProvider.GetMediaLibraries();
    // }
}