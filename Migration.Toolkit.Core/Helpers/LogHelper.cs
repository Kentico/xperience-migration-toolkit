using Migration.Toolkit.Common.Helpers;

namespace Migration.Toolkit.Core.Helpers;

public static class LogHelper
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
            
            null => $"{currentTypeName}: <null>",
            _ => $"TODO: {typeof(T).FullName}"
        };
    }
}