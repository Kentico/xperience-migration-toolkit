using CMS.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Migration.Toolkit.KXO.Api;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection UseKxoApi(this IServiceCollection services, IConfiguration configuration, string? applicationPhysicalPath = null)
    {
        Service.Use<IConfiguration>(configuration);
        if (applicationPhysicalPath != null && Directory.Exists(applicationPhysicalPath))
        {
            CMS.Base.SystemContext.WebApplicationPhysicalPath = applicationPhysicalPath;    
        }

        services.AddSingleton<KxoApiInitializer>();

        services.AddSingleton<KxoClassFacade>();
        services.AddSingleton<KxoFormFacade>();
        services.AddSingleton<KxoMediaFileFacade>();
        services.AddSingleton<KxoPageFacade>();

        return services;
    }
}