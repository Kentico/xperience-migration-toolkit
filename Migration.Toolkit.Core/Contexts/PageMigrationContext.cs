using Migration.Toolkit.KX13.Models;

namespace Migration.Toolkit.Core.Contexts;

public class PageMigrationContext
{
    private record PageInfo(Guid? DocumentGuid, int DocumentId, int NodeId, MigrationState State);

    private enum MigrationState
    {
        Skipped,
        Failed,
        Success
    }

    private readonly HashSet<PageInfo> _pageMigrationStatuses = new();

    public void AddSkippedPage(CmsDocument cmsDocument)
    {
        _pageMigrationStatuses.Add(new(cmsDocument.DocumentGuid, cmsDocument.DocumentId, cmsDocument.DocumentNodeId, MigrationState.Skipped));
    }

    public void AddFailedPage(CmsDocument cmsDocument)
    {
        _pageMigrationStatuses.Add(new(cmsDocument.DocumentGuid, cmsDocument.DocumentId, cmsDocument.DocumentNodeId, MigrationState.Failed));
    }

    public void AddSuccessfullyMigratedPage(CmsDocument cmsDocument)
    {
        _pageMigrationStatuses.Add(new(cmsDocument.DocumentGuid, cmsDocument.DocumentId, cmsDocument.DocumentNodeId, MigrationState.Success));
    }

    public bool CheckParent(int parentNodeId)
    {
        var parentInfo = _pageMigrationStatuses.FirstOrDefault(ps => ps.NodeId == parentNodeId);
        return parentInfo != null && parentInfo.State == MigrationState.Success;
    }
}