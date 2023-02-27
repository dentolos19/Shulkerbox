﻿using System;
using Windows.System;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Navigation;
using Shulkerbox.Models;

namespace Shulkerbox.Pages;

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