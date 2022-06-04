using CMS.FormEngine;
using CMS.OnlineForms;

namespace Migration.Toolkit.Core.Convertors;

public class BizFormDefinitionConvertor
{
    public void Convert(string formDefinitionXml)
    {
        // BizFormInfoProvider.GetBizFormFileColumns()

        var formInfo = new FormInfo(formDefinitionXml);
        
        // formInfo.
    }
}