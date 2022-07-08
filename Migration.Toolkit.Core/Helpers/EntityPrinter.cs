using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.MediaLibrary;
using Migration.Toolkit.Common.Helpers;

namespace Migration.Toolkit.Core.Helpers;

public static class EntityPrinter
{
    public static string PrintKxoModelInfo<T>(T model)
    {
        var currentTypeName = ReflectionHelper<T>.CurrentType.Name;

        return model switch
        {
            KXOM.CmsSite site => $"{currentTypeName}: {nameof(site.SiteGuid)}={site.SiteGuid}",
            KXOM.MediaLibrary mediaLibrary => $"{currentTypeName}: {nameof(mediaLibrary.LibraryGuid)}={mediaLibrary.LibraryGuid}",
            KXOM.MediaFile mediaFile => $"{currentTypeName}: {nameof(mediaFile.FileGuid)}={mediaFile.FileGuid}",
            KXOM.CmsTree tree => $"{currentTypeName}: {nameof(tree.NodeGuid)}={tree.NodeGuid}",
            KXOM.CmsDocument document => $"{currentTypeName}: {nameof(document.DocumentGuid)}={document.DocumentGuid}",
            KXOM.CmsAcl acl => $"{currentTypeName}: {nameof(acl.Aclguid)}={acl.Aclguid}",
            KXOM.CmsRole role => $"{currentTypeName}: {nameof(role.RoleGuid)}={role.RoleGuid}, {nameof(role.RoleName)}={role.RoleName}",
            KXOM.CmsUser user => $"{currentTypeName}: {nameof(user.UserGuid)}={user.UserGuid}, {nameof(user.UserName)}={user.UserName}",
            KXOM.CmsResource resource => $"{currentTypeName}: {nameof(resource.ResourceGuid)}={resource.ResourceGuid}, {nameof(resource.ResourceName)}={resource.ResourceName}",
            KXOM.CmsSettingsCategory settingsCategory => $"{currentTypeName}: {nameof(settingsCategory.CategoryName)}={settingsCategory.CategoryName}",
            KXOM.CmsSettingsKey settingsKey => $"{currentTypeName}: {nameof(settingsKey.KeyGuid)}={settingsKey.KeyGuid}, {nameof(settingsKey.KeyName)}={settingsKey.KeyName}",
            KXOM.CmsForm form => $"{currentTypeName}: {nameof(form.FormGuid)}={form.FormGuid}, {nameof(form.FormName)}={form.FormName}",
            KXOM.CmsPageUrlPath pageUrlPath => $"{currentTypeName}: {nameof(pageUrlPath.PageUrlPathGuid)}={pageUrlPath.PageUrlPathGuid}, {nameof(pageUrlPath.PageUrlPathUrlPath)}={pageUrlPath.PageUrlPathUrlPath}",
            KXOM.OmContactGroup omContactGroup => $"{currentTypeName}: {nameof(omContactGroup.ContactGroupGuid)}={omContactGroup.ContactGroupGuid}, {nameof(omContactGroup.ContactGroupName)}={omContactGroup.ContactGroupName}",
            
            null => $"{currentTypeName}: <null>",
            _ => $"TODO: {typeof(T).FullName}"
        };
    }

    public static string GetEntityIdentityPrint<T>(T model)
    {
        var currentTypeName = ReflectionHelper<T>.CurrentType.Name;

        return model switch
        {
            MediaLibraryInfo mfi => $"ID={mfi.LibraryID}, GUID={mfi.LibraryGUID}, Name={mfi.LibraryName}",
            MediaFileInfo mf => $"ID={mf.FileID}, GUID={mf.FileGUID}, Name={mf.FileName}",
            DataClassInfo dci => $"ID={dci.ClassID}, GUID={dci.ClassGUID}, Name={dci.ClassName}",
            PageUrlPathInfo pupi => $"ID={pupi.PageUrlPathID}, GUID={pupi.PageUrlPathGUID}, UrlPath={pupi.PageUrlPathUrlPath}",
            TreeNode tn => $"NodeID={tn.NodeID}, DocumentID={tn.DocumentID}, NodeGUID={tn.NodeGUID}, DocumentGUID={tn.DocumentGUID}, DocumentCulture={tn.DocumentCulture}, DocumentName={tn.DocumentName}",
                
            KXOM.CmsForm cf => $"ID={cf.FormId}, GUID={cf.FormGuid}, Name={cf.FormName}",
            
            KX13M.CmsAttachment ca13 => $"ID={ca13.AttachmentId}, GUID={ca13.AttachmentGuid}, Name={ca13.AttachmentName}",
            
            null => $"<null> of {currentTypeName}",
            _ => $"TODO: {ReflectionHelper<T>.CurrentType.FullName}"
        };
        
        // throw new NotImplementedException($"No entity identity print defined for type '{ReflectionHelper<T>.CurrentType.FullName}'");
    }
}