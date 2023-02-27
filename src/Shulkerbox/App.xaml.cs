using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Shulkerbox.Services;

namespace Shulkerbox;

public partial class App
{
    public IServiceProvider Services { get; private set; }
    public MainWindow MainWindow { get; private set; }

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        Services = ConfigureServices();
        MainWindow = new MainWindow();
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

    public static async Task<ContentDialogResult> AttachDialog(ContentDialog dialog)
    {
        dialog.XamlRoot = ((App)Current).MainWindow.Content.XamlRoot;
        return await dialog.ShowAsync();
    }

    public static async Task<bool> AttachDialog(string message, string? title = null, bool isOption = false)
    {
        var dialog = new ContentDialog { Content = message, CloseButtonText = "Close" };
        if (!string.IsNullOrEmpty(title))
            dialog.Title = title;
        if (isOption)
        {
            dialog.PrimaryButtonText = "Yes";
            dialog.CloseButtonText = "No";
        }
        return await AttachDialog(dialog) == ContentDialogResult.Primary;
    }
}