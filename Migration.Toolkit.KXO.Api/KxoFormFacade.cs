using CMS.OnlineForms;
using CMS.SiteProvider;

namespace Migration.Toolkit.KXO.Api;

public class KxoFormFacade
{
    public void SetForm(string formDisplayName, string formName, string tableName, SiteInfo siteInfo)
    {
        // new TableManager().CreateTable();
        // BizFormInfoProvider
        BizFormHelper.Create(formDisplayName, formName, tableName, siteInfo);
        // BizFormInfoProvider.CreateBizFormDataClass();
        // BizFormInfoProvider.CreateBizFormDataClass()
    }
    
    public BizFormInfo GetForm(Guid formGuid, int siteId)
    {
        return BizFormInfoProvider.GetBizFormInfoByGUID(formGuid, siteId);
    }
}