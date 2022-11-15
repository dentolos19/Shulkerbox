using System;
using System.Linq;
using CraftMine.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CraftMine;

public sealed partial class MainWindow
{

    public MainWindow()
    {
        InitializeComponent();
        Title = "Shulkerbox";
        NavigationView.RequestedTheme = ElementTheme.Dark;
        NavigationView.MenuItems.Add(new NavigationViewItem
        {
            Icon = new SymbolIcon(Symbol.Home),
            Content = "Home",
            Tag = typeof(HomePage)
        });
        NavigationView.MenuItems.Add(new NavigationViewItem
        {
            Icon = new SymbolIcon(Symbol.Library),
            Content = "Versions",
            Tag = typeof(VersionsPage)
        });
        NavigationView.MenuItems.Add(new NavigationViewItem
        {
            Icon = new SymbolIcon(Symbol.People),
            Content = "Accounts",
            Tag = typeof(AccountsPage)
        });
        NavigationView.SelectedItem = NavigationView.MenuItems.First();
    }

    private void OnNavigateRequested(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentView.Navigate(typeof(SettingsPage));
        }
        else
        {
            if (args.SelectedItem is NavigationViewItem { Tag: Type type })
                ContentView.Navigate(type);
        }
    }

    private void OnContentNavigating(object sender, NavigatingCancelEventArgs args)
    {
        if (NavigationView.SelectedItem is not NavigationViewItem { Tag: Type type })
            return;
        if (type != args.SourcePageType)
            NavigationView.SelectedItem = null;
    }

}