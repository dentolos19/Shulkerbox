using System;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace CraftMine.Pages;

public sealed partial class MainPage
{

    public MainPage()
    {
        InitializeComponent();
        NavigationView.MenuItems.Add(new NavigationViewItem
        {
            Icon = new SymbolIcon(Symbol.Home),
            Content = "Home",
            Tag = typeof(HomePage)
        });
        NavigationView.MenuItems.Add(new NavigationViewItem
        {
            Icon = new SymbolIcon(Symbol.Contact),
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

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (ContentView.CanGoBack)
            ContentView.GoBack();
    }

    private void OnContentNavigating(object sender, NavigatingCancelEventArgs args)
    {
        if (NavigationView.SelectedItem is not NavigationViewItem { Tag: Type type })
            return;
        if (type != args.SourcePageType)
            NavigationView.SelectedItem = null;
    }

}