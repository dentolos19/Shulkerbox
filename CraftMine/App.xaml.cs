using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using CmlLib.Core;
using CraftMine.Core;
using CraftMine.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CraftMine;

public partial class App
{

    private static Window _mainWindow;

    internal static Settings Settings { get; } = Settings.Load();
    internal static CMLauncher Launcher { get; } = new(Path.Combine(ApplicationData.Current.LocalFolder.Path, "game"));

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var frame = new Frame();
        _mainWindow = new Window { Title = Package.Current.DisplayName, Content = frame };
        frame.Navigate(typeof(MainPage));
        _mainWindow.Activate();
    }

    internal static async Task AttachDialog(string message, string? title = null)
    {
        var dialog = new ContentDialog { Content = message, PrimaryButtonText = "Close" };
        if (!string.IsNullOrEmpty(title))
            dialog.Title = title;
        dialog.XamlRoot = _mainWindow.Content.XamlRoot;
        await dialog.ShowAsync();
    }

    internal static async Task AttachDialog(ContentDialog dialog)
    {
        dialog.XamlRoot = _mainWindow.Content.XamlRoot;
        await dialog.ShowAsync();
    }

}