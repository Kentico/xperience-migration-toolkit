using System.Linq.Expressions;

namespace Migration.Toolkit.Common;

public class ToolkitConfiguration
{
    public string SourceConnectionString { get; set; }
    public string TargetConnectionString { get; set; }
    public EntityConfigurations EntityConfigurations { get; set; }
    
    public string TargetAttachmentMediaLibraryName { get; set; }

    public Dictionary<int?, int?> RequireSiteIdExplicitMapping<TEntityType>(Expression<Func<TEntityType, object>> keyNameSelector)
    {
        var memberName = keyNameSelector.GetMemberName();
        var explicitSiteIdMapping = EntityConfigurations?.GetEntityConfiguration<TEntityType>()?.ExplicitPrimaryKeyMapping[memberName];
        if (explicitSiteIdMapping == null)
        {
            throw new InvalidOperationException($"{typeof(TEntityType).Name} ExplicitPrimaryKeyMapping of {memberName} is required.");
        }

        return explicitSiteIdMapping.ToDictionary(kvp => (int?)int.Parse(kvp.Key), kvp => kvp.Value);
    }
}