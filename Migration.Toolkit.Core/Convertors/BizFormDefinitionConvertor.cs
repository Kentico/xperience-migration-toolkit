using CMS.DataEngine;
using CMS.FormEngine;
using CMS.OnlineForms;

namespace Migration.Toolkit.Core.Convertors;

public static class BizFormDefinitionConvertor
{
    public static string ConvertToKxo(string formDefinitionXml)
    {
        // BizFormInfoProvider.GetBizFormFileColumns()
        
        var formInfo = new FormInfo(formDefinitionXml);
        foreach (var columnName in formInfo.GetColumnNames())
        {
            var field = formInfo.GetFormField(columnName);
        }

        foreach (var dataDefinitionItem in formInfo.ItemsList)
        {
            switch (dataDefinitionItem)
            {
                case IField field:
                {
                    
                    break;
                }
                // case CMS.DataEngine.FieldInfo fieldInfo:
                // {
                //     fieldInfo.
                //     break;
                // }
                default:
                {
                    // ConvertToKxo()
                    break;
                }
            }
        }

        return formInfo.GetXmlDefinition();
    }
}