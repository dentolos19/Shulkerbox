using System.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Shulkerbox.Services;
using Shulkerbox.Shared;

namespace Shulkerbox;

public partial class App
{
    private void OnStartup(object sender, StartupEventArgs args)
    {
        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.VisibleStateDuration = 1000;
            config.SnackbarConfiguration.ShowTransitionDuration = 250;
            config.SnackbarConfiguration.HideTransitionDuration = 250;
        });
        services.AddMudMarkdownServices();
        services.AddSingleton(new LayoutService());
        services.AddSingleton(new GameService());
        services.AddSingleton(SettingsService.Initialize());
        new Window
        {
            Title = "Shulkerbox",
            Content = new BlazorWebView
            {
                HostPage = "wwwroot/index.html",
                Services = services.BuildServiceProvider(),
                RootComponents =
                {
                    new RootComponent
                    {
                        Selector = "#app",
                        ComponentType = typeof(Root)
                    }
                }
            }
        }.Show();
    }
}