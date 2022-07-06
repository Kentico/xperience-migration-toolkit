using CMS.OnlineForms;
using CMS.SiteProvider;
using Microsoft.Extensions.Logging;

namespace Migration.Toolkit.KXO.Api;

public class KxoFormFacade
{
    private readonly ILogger<KxoClassFacade> _logger;
    private readonly KxoApiInitializer _kxoApiInitializer;

    public KxoFormFacade(ILogger<KxoClassFacade> logger, KxoApiInitializer kxoApiInitializer)
    {
        _logger = logger;
        _kxoApiInitializer = kxoApiInitializer;

        _kxoApiInitializer.EnsureApiIsInitialized();
    }
    
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