using System.Diagnostics;
using CMS.DataEngine;
using CMS.OnlineForms;
using Microsoft.Extensions.Logging;
using TableManager = CMS.DataProviderSQL.TableManager;

namespace Migration.Toolkit.KXO.Api;

public class KxoClassFacade
{
    private readonly ILogger<KxoClassFacade> _logger;
    private readonly KxoApiInitializer _kxoApiInitializer;

    public KxoClassFacade(ILogger<KxoClassFacade> logger, KxoApiInitializer kxoApiInitializer)
    {
        _logger = logger;
        _kxoApiInitializer = kxoApiInitializer;

        _kxoApiInitializer.EnsureApiIsInitialized();
    }
    
    public DataClassInfo SetClass(DataClassInfo dataClassInfo)
    {
        // TODO tk: 2022-05-30  dataClassInfo.ClassConnectionString - check, might cause unexpected behavior
        DataClassInfoProvider.SetDataClassInfo(dataClassInfo);
        Debug.Assert(dataClassInfo.ClassID != 0, "dataClassInfo.ClassID != 0");
        return dataClassInfo;
    }

    public DataClassInfo GetClass(Guid classGuid)
    {
        return DataClassInfoProvider.GetDataClassInfo(classGuid);
    }
}