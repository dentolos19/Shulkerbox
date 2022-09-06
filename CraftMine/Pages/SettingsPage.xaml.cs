using System;
using CommunityToolkit.WinUI.UI.Controls;
using CraftMine.Models;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;

namespace CraftMine.Pages;

public sealed partial class SettingsPage
{

    private SettingsPageModel Context => (SettingsPageModel)DataContext;

    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs args)
    {
        await Context.LoadCommand.ExecuteAsync(null);
    }

    protected override async void OnNavigatedFrom(NavigationEventArgs args)
    {
        await Context.SaveCommand.ExecuteAsync(null);
    }

    private async void OnLinkClicked(object? sender, LinkClickedEventArgs args)
    {
        await Launcher.LaunchUriAsync(new Uri(args.Link));
    }

}