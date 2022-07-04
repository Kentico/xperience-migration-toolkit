namespace Migration.Toolkit.Core.MigrationProtocol;

public static class HandbookReferences
{
    #region "Warnings - nothing needs to be done"

    public static HandbookReference EntityExplicitlyExcludedByCodeName(string codeName, string entityName) => new("EntityExplicitlyExcludedByCodeName", $"CodeName={codeName}, EntityName={entityName}");
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

    public static HandbookReference CreatePossiblyCustomControlNeedToBeMigrated(string controlName) => new("ClassFormDefinition_PossiblyCustomControlNeedsToBeCreated", controlName);

    public static HandbookReference SourcePageIsNotPublished(Guid sourcePageGuid) => new("SourcePageIsNotPublished", $"PageGuid={sourcePageGuid}"); 
    
    public static HandbookReference CmsTreeTreeIsLinkFromDifferentSite => new("CmsTree_TreeIsLinkFromDifferentSite");
    
    #endregion


    #region "Not supported right now"

    public static HandbookReference MediaFileMigrateFileManually => new("MediaFile_MigrateFileManually");

    #endregion

    #region "Errors - something need to be done"

    public static HandbookReference CmsUserEmailConstraintBroken => new("CmsUser_EmailConstraintBroken");
    public static HandbookReference CmsUserUserNameConstraintBroken => new("CmsUser_UserNameConstraintBroken");
    public static HandbookReference CmsTreeTreeRootIsMissing => new("CmsTree_TreeRootIsMissing");
    public static HandbookReference CmsTreeUserIsMissingInTargetInstance => new("CmsTree_UserIsMissingInTargetInstance");
    public static HandbookReference CmsTreeTreeParentIsMissing => new("CmsTree_TreeParentIsMissing");
    public static HandbookReference BulkCopyColumnMismatch(string tableName) => new("BulkCopyColumnMismatch", $"TableName={tableName}");

    #endregion
}