using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Shulkerbox;

public static class ShulkExtensions
{
    public static IServiceCollection AddBlazorServices(this IServiceCollection services)
    {
        return services
            .AddMudServices()
            .AddSingleton(new ShulkLauncher())
            .AddSingleton(ShulkSettings.Load());
    }
}