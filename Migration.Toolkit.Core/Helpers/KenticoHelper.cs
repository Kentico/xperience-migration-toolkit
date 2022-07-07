using CMS.Helpers;

namespace Migration.Toolkit.Core.Helpers;

public static class KenticoHelper
{
    public static void CopyCustomData(ContainerCustomData target, string? sourceXml)
    {
        var customNodeData = new ContainerCustomData();
        customNodeData.LoadData(sourceXml);
        foreach (var columnName in customNodeData.ColumnNames)
        {
            target.SetValue(columnName, customNodeData.GetValue(columnName));
        }
    }
}