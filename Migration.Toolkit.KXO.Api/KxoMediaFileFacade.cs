using System.Diagnostics;
using CMS.DataEngine;
using CMS.MediaLibrary;
using Microsoft.Extensions.Logging;

namespace Migration.Toolkit.KXO.Api;

public class KxoMediaFileFacade
{
    private readonly ILogger<KxoMediaFileFacade> _logger;
    public KxoMediaFileFacade(ILogger<KxoMediaFileFacade> logger, KxoApiInitializer kxoApiInitializer)
    {
        _logger = logger;
        kxoApiInitializer.EnsureApiIsInitialized();
    }

    public void SetMediaFile(MediaFileInfo mfi, bool newInstance)
    {
        Debug.Assert((newInstance && mfi.FileID == 0) || (!newInstance && mfi.FileID != 0), "newInstance && mfi.FileID == 0");
        
        if (newInstance)
        {
            mfi.Insert();
            mfi.SaveFileToDisk(true);
        }
        else
        {
            mfi.Update();
        }
    }
    
    public MediaFileInfo? GetMediaFile(Guid mediaFileGuid)
    {
        return MediaFileInfoProvider.GetMediaFiles("").Where(nameof(MediaFileInfo.FileGUID), QueryOperator.Equals, mediaFileGuid).SingleOrDefault();
    }

    public MediaLibraryInfo GetMediaLibraryInfo(Guid mediaLibraryGuid)
    {
        return MediaLibraryInfoProvider.ProviderObject.Get(mediaLibraryGuid);
    }
    
    public void EnsureMediaFilePathExistsInLibrary(MediaFileInfo mfi, int libraryId, string siteName)
    {
        var librarySubDir = Path.GetDirectoryName(mfi.FilePath);
        MediaLibraryInfoProvider.CreateMediaLibraryFolder(siteName, libraryId, librarySubDir, false, false);
    }
    
    public MediaLibraryInfo CreateMediaLibrary(int siteId, string libraryFolder, string libraryDescription, string libraryName, string libraryDisplayName)
    {
        // Creates a new media library object
        MediaLibraryInfo newLibrary = new MediaLibraryInfo();

        // Sets the library properties
        newLibrary.LibraryDisplayName = libraryDisplayName;
        newLibrary.LibraryName = libraryName;
        newLibrary.LibraryDescription = libraryDescription;
        newLibrary.LibraryFolder = libraryFolder;
        newLibrary.LibrarySiteID = siteId;

        // Saves the new media library to the database
        MediaLibraryInfo.Provider.Set(newLibrary);
        
        return newLibrary; 
    }
    
    public MediaLibraryInfo SetMediaLibrary(MediaLibraryInfo mfi)
    {
        MediaLibraryInfo.Provider.Set(mfi);
        
        Debug.Assert(mfi.LibraryID != 0, "mfi.LibraryID != 0");
        
        return mfi; 
    }
}