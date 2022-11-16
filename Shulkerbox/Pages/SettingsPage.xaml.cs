﻿using System.Windows;
using Shulkerbox.Models;

namespace Shulkerbox.Pages;

public partial class SettingsPage
{

    private SettingsPageModel Model => (SettingsPageModel)DataContext;

    public SettingsPage()
    {
        InitializeComponent();
        DataContext = App.GetModel<SettingsPageModel>();
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        Model.RefreshCommand.Execute(null);
    }

}