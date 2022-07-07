using System.Linq.Expressions;

namespace Migration.Toolkit.Common;

public enum AutofixEnum
{
    DiscardData,
    AttemptFix,
    Error,
}

public class ToolkitConfiguration
{
    public string? SourceConnectionString { get; set; }
    public string? SourceCmsDirPath { get; set; }
    
    public string? TargetConnectionString { get; set; }
    public string? TargetCmsDirPath { get; set; }
    
    public EntityConfigurations? EntityConfigurations { get; set; }
    public string? TargetAttachmentMediaLibraryName { get; set; }
    public bool? MigrateOnlyMediaFileInfo { get; set; } = true;

    public AutofixEnum? UseOmActivityNodeRelationAutofix { get; set; } = AutofixEnum.Error;
    public AutofixEnum? UseOmActivitySiteRelationAutofix { get; set; } = AutofixEnum.Error;

    public Dictionary<int?, int?> RequireSiteIdExplicitMapping<TEntityType>(Expression<Func<TEntityType, object>> keyNameSelector)
    {
        var memberName = keyNameSelector.GetMemberName();
        var migratedSiteIds = EntityConfigurations?.GetEntityConfiguration<TEntityType>()?.ExplicitPrimaryKeyMapping[memberName];
        if (migratedSiteIds == null)
        {
            throw new InvalidOperationException($"{typeof(TEntityType).Name} ExplicitPrimaryKeyMapping of {memberName} is required.");
        }

        return migratedSiteIds.ToDictionary(kvp => (int?)int.Parse(kvp.Key), kvp => kvp.Value);
    }
}