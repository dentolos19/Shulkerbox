using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Shulkerbox.Models;
using Shulkerbox.Services;
using Shulkerbox.Pages;

namespace Shulkerbox;

public partial class App
{

    private static Window _window;
    private static ServiceProvider _services;
    private static ServiceProvider _modelServices;

    public App()
    {
        var services = new ServiceCollection();
        services.AddSingleton<SettingsService>(SettingsService.Initialize());
        services.AddSingleton<GameService>();
        _services = services.BuildServiceProvider();
        var modelServices = new ServiceCollection();
        modelServices.AddSingleton<MainPageModel>();
        modelServices.AddSingleton<SettingsPageModel>();
        _modelServices = modelServices.BuildServiceProvider();
    }

    private void OnStartup(object sender, StartupEventArgs args)
    {
        _window = new Window
        {
            Title = "Shulkerbox",
            ResizeMode = ResizeMode.CanMinimize,
            Width = 500,
            Height = 300,
            Content = new Frame
            {
                NavigationUIVisibility = NavigationUIVisibility.Hidden
            }
        };
        Navigate(nameof(MainPage));
        _window.Show();
    }

    public static TService? GetService<TService>()
    {
        return _services.GetService<TService>();
    }

    public static TModel? GetModel<TModel>()
    {
        return _modelServices.GetService<TModel>();
    }

    public static void Navigate(string pageName)
    {
        ((Frame)_window.Content).Navigate(new Uri($"Pages/{pageName}.xaml", UriKind.Relative));
    }

    public static void NavigateBack()
    {
        ((Frame)_window.Content).GoBack();
    }

}