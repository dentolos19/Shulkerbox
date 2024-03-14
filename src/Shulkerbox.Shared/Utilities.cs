using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared;

public static class Utilities
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        return services
            .AddMudServices()
            .AddMudMarkdownServices()
            .AddSingleton(GameService.Initialize())
            .AddSingleton(LayoutService.Initialize())
            .AddSingleton(ResourceService.Initialize())
            .AddSingleton(SettingsService.Initialize());
    }

    public static void ExecuteShell(string path)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        });
    }
}