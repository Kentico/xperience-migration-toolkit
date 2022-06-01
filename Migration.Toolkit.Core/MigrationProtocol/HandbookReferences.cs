namespace Migration.Toolkit.Core.MigrationProtocol;

public static class HandbookReferences
{
    #region "Warnings - nothing needs to be done"

    public static HandbookReference CmsClassCmsRootClassTypeSkip => new("CmsClass_CmsRootClassTypeSkip");
    public static HandbookReference CmsClassClassConnectionStringIsDifferent => new("CmsClass_ClassConnectionStringIsDifferent");
    public static HandbookReference CmsUserAdminUserSkip => new("CmsUser_SkipAdminUser");
    public static HandbookReference CmsUserPublicUserSkip => new("CmsUser_SkipPublicUser");
    public static HandbookReference CmsWebFarmServerSkip => new("CmsWebFarm_SkipPublicWebFarm");
    public static HandbookReference CmsConsentSkip => new("CmsConsent_SkipPublicConsent");
    public static HandbookReference CmsConsentArchiveSkip => new("CmsConsentArchive_SkipPublicConsentArchive");
    public static HandbookReference CmsConsentAgreementSkip => new("CmsConsentAgreement_SkipPublicConsentAgreement");
    public static HandbookReference CmsTreeTreeRootSkip => new("CmsTree_TreeRootSkipped");
    public static HandbookReference CmsSettingsKeyExclusionListSkip => new("CmsSettingsKey_SkipExclusionList");
    
    #endregion


    #region "Not supported right now"

    public static HandbookReference MediaFileMigrateFileManually => new("MediaFile_MigrateFileManually");

    #endregion

    #region "Errors - something need to be done"

    public static HandbookReference CmsUserEmailConstraintBroken => new("CmsUser_EmailConstraintBroken");
    public static HandbookReference CmsUserUserNameConstraintBroken => new("CmsUser_UserNameConstraintBroken");
    public static HandbookReference CmsTreeTreeRootIsMissing => new("CmsTree_TreeRootIsMissing");

    #endregion
}