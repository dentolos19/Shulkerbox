using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Shulkerbox;

public static class ShulkExtensions
{
    public static IServiceCollection AddBlazorServices(this IServiceCollection services)
    {
        return services
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.VisibleStateDuration = 2000;
                config.SnackbarConfiguration.ShowTransitionDuration = 250;
                config.SnackbarConfiguration.HideTransitionDuration = 250;
            })
            .AddSingleton(new ShulkLauncher())
            .AddSingleton(ShulkSettings.Load());
    }
}