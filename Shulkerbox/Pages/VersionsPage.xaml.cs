using CraftMine.Models;
using Microsoft.UI.Xaml.Navigation;

namespace CraftMine.Pages;

public sealed partial class VersionsPage
{

    private VersionsPageModel Context => (VersionsPageModel)DataContext;

    public VersionsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs args)
    {
        Context.RefreshCommand.Execute(null);
    }

}