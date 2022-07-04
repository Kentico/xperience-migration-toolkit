using CMS.Helpers;
using CMS.MediaLibrary;
using Migration.Toolkit.Core.Contexts;

namespace Migration.Toolkit.Core.Convertors;

public class AttachmentConvertor
{
    private readonly PrimaryKeyMappingContext _primaryKeyMappingContext;

    public AttachmentConvertor(PrimaryKeyMappingContext primaryKeyMappingContext)
    {
        _primaryKeyMappingContext = primaryKeyMappingContext;
    }
    
    public MediaFileInfo ToMediaFile(KX13.Models.CmsAttachment source, int targetLibraryId)
    {
        var target = new MediaFileInfo();
        
        // target.FileId = source.FileId;
        target.FileName = source.AttachmentName;
        target.FileTitle = source.AttachmentTitle;
        target.FileDescription = source.AttachmentDescription;
        target.FileExtension = source.AttachmentExtension;
        target.FileMimeType = source.AttachmentMimeType;
        // target.FilePath = source.path;
        target.FileSize = source.AttachmentSize;
        target.FileImageWidth = source.AttachmentImageWidth ?? 0;
        target.FileImageHeight = source.AttachmentImageHeight ?? 0;
        target.FileGUID = source.AttachmentGuid;
        target.FileLibraryID = targetLibraryId;
        target.FileSiteID = _primaryKeyMappingContext.RequireMapFromSource<KX13.Models.CmsSite>(s => s.SiteId, source.AttachmentSiteId);

        // TODO tk: 2022-06-29 user not migrated
        // target.FileCreatedByUserID = source.;
        // target.FileModifiedByUserID = source.FileModifiedByUserId;

        // TODO tk: 2022-06-29 temporal info not migrated
        // target.FileCreatedWhen = source.;
        // target.FileModifiedWhen = source.FileModifiedWhen;

        // TODO tk: 2022-06-29 custom data not migrated
        // target.FileCustomData = source.AttachmentCustomData;

        
        return target;
    }
}