using System.Diagnostics;
using System.Reflection;
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
            .AddSingleton(AuthenticationService.Initialize())
            .AddSingleton(GameService.Initialize())
            .AddSingleton(LayoutService.Initialize())
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

    public static async Task<string> GetResourceStringAsync(string resourceName)
    {
        await using var stream =
            Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"Shulkerbox.Shared.Resources.{resourceName}");
        using var reader = new StreamReader(stream!);
        return await reader.ReadToEndAsync();
    }
}