using System;
using System.Threading.Tasks;
using CmlLib.Core;
using CraftMine.Core;
using CraftMine.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CraftMine;

public partial class App
{

    private static Window _mainWindow;

    internal static CMLauncher Launcher { get; } = new(new MinecraftPath());
    internal static Settings Settings { get; } = Settings.Load();

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _mainWindow = new Window
        {
            Title = "CraftMine",
            Content = new Frame()
        };
        NavigateFrame(typeof(MainPage));
        _mainWindow.Activate();
    }

    internal static void NavigateFrame(Type type, object? parameter = null)
    {
        if (_mainWindow is not { Content: Frame frame })
            return;
        if (parameter == null)
            frame.Navigate(type);
        else
            frame.Navigate(type, parameter);
    }

    internal static async Task AttachDialog(string message, string? title = null)
    {
        var dialog = new ContentDialog { Content = message, CloseButtonText = "Close" };
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