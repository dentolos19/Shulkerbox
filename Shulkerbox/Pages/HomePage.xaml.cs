using CraftMine.Models;
using Microsoft.UI.Xaml.Navigation;

namespace CraftMine.Pages;

public sealed partial class HomePage
{

    private HomePageModel Context => (HomePageModel)DataContext;

    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs args)
    {
        Context.ReloadCommand.Execute(null);
    }

}