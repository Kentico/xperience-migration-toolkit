using Migration.Toolkit.Common.Helpers;

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
    
    public static HandbookReference TemporaryAttachmentMigrationIsNotSupported => new("TemporaryAttachmentMigrationIsNotSupported");

    public static HandbookReference LinkedDataAlreadyMaterializedInTargetInstance =>
        new HandbookReference("LinkedDataAlreadyMaterializedInTargetInstance");
    
    #endregion


    #region "Errors - something need to be done"
    
    public static HandbookReference MediaFileIsMissingOnSourceFilesystem => new("MediaFileIsMissingOnSourceFilesystem");

    public static HandbookReference MissingRequiredDependency<TSourceDependency>(string fieldName, object? sourceValue) =>
        new HandbookReference("MissingRequiredDependency")
            .WithData(new
            {
                DependencyType = typeof(TSourceDependency).Name,
                FieldName = fieldName,
                SourceValue = sourceValue
            });

    public static HandbookReference SourceEntityIsNull<TSource>() =>
        new HandbookReference("SourceEntityIsNull")
            .WithData(new
            {
                SourceEntityType = typeof(TSource).FullName,
            });
    
    public static HandbookReference SourceValueIsRequired<TSource>(string valueName) =>
        new HandbookReference("SourceValueIsRequired")
            .NeedsManualAction()
            .WithData(new
            {
                SourceEntityType = typeof(TSource).FullName,
                ValueName = valueName
            });
    
    public static HandbookReference FailedToCreateTargetInstance<TTarget>() =>
        new HandbookReference("FailedToCreateTargetInstance")
            .NeedsManualAction()
            .WithData(new
            {
                TargetEntityType = typeof(TTarget).FullName,                
            });
    
    public static HandbookReference ErrorCreatingTargetInstance<TTarget>(Exception exception) =>
        new HandbookReference("FailedToCreateTargetInstance")
            .NeedsManualAction()
            .WithData(new
            {
                TargetEntityType = typeof(TTarget).FullName,
                Exception = exception.ToString(),
                
            });
    
    public static HandbookReference MissingConfiguration<TCommand>(string configurationName) =>
        new HandbookReference("MissingConfiguration")
            .NeedsManualAction()
            .WithData(new
            {
                Command = typeof(TCommand).FullName, 
                ConfigurationName = configurationName
            });
    
    public static HandbookReference CmsUserEmailConstraintBroken => new("CmsUser_EmailConstraintBroken");
    public static HandbookReference CmsUserUserNameConstraintBroken => new("CmsUser_UserNameConstraintBroken");
    public static HandbookReference CmsTreeTreeRootIsMissing => new("CmsTree_TreeRootIsMissing");
    public static HandbookReference CmsTreeUserIsMissingInTargetInstance => new("CmsTree_UserIsMissingInTargetInstance");
    public static HandbookReference CmsTreeTreeParentIsMissing => new("CmsTree_TreeParentIsMissing");
    public static HandbookReference BulkCopyColumnMismatch(string tableName) => new("BulkCopyColumnMismatch", $"TableName={tableName}");

    public static HandbookReference DataMustNotExistInTargetInstanceTable(string tableName) =>
        new HandbookReference("DataMustNotExistInTargetInstanceTable")
            .WithData(new { tableName });

    public static HandbookReference ValueTruncationSkip(string tableName) =>
        new HandbookReference("ValueTruncationSkip").NeedsManualAction().WithData(new { tableName });

    public static HandbookReference NotCurrentlySupportedSkip<TSource>() =>
        new HandbookReference("NotCurrentlySupportedSkip").WithData(new
        {
            Type = ReflectionHelper<TSource>.CurrentType.Name
        });
    
    public static HandbookReference FaultyData<TSource>() => new HandbookReference("FaultyData")
        .NeedsManualAction()
        .WithData(new
        {
            SourceEntityType = typeof(TSource).Name,
        });

    #endregion
}