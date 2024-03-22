using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaBlazorWebView;
using Shulkerbox.Shared;

namespace Shulkerbox.Desktop;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();
        AvaloniaBlazorWebViewBuilder.Initialize(
            config =>
            {
#if !DEBUG
                config.AreDevToolEnabled = false;
#endif
            },
            settings =>
            {
                settings.Selector = "#app";
                settings.ComponentType = typeof(Root);
                settings.ResourceAssembly = typeof(Root).Assembly;
            },
            services =>
            {
                services.AddBlazorServices();
            }
        );
    }

    public override void OnFrameworkInitializationCompleted()
    {
        switch (ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime desktop:
                desktop.MainWindow = new MainWindow();
                break;
            case ISingleViewApplicationLifetime mobile:
                mobile.MainView = new MainView();
                break;
        }
        base.OnFrameworkInitializationCompleted();
    }
}