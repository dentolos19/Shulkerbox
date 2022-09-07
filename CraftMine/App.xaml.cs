using System;
using Windows.Foundation;
using CraftMine.Pages;
using CraftMine.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CraftMine;

public partial class App
{

    public IServiceProvider Services { get; private set; }
    public Window MainWindow { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Services = ConfigureServices();
        MainWindow = new Window
        {
            Title = "CraftMine",
            Content = new Frame { Content = new MainPage() }
        };
        MainWindow.Activate();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton(SettingsService.Initialize());
        services.AddSingleton<GameService>();
        return services.BuildServiceProvider();
    }

    public static TService? GetService<TService>()
    {
        return (TService?)((App)Current).Services.GetService(typeof(TService));
    }

    public static IAsyncOperation<ContentDialogResult> AttachDialog(ContentDialog dialog)
    {
        dialog.XamlRoot = ((App)Current).MainWindow.Content.XamlRoot;
        return dialog.ShowAsync();
    }

    public static IAsyncOperation<ContentDialogResult> AttachDialog(string message, string? title = null)
    {
        var dialog = new ContentDialog { Content = message, CloseButtonText = "Close" };
        if (!string.IsNullOrEmpty(title))
            dialog.Title = title;
        return AttachDialog(dialog);
    }

}