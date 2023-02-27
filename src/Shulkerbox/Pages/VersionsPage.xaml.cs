using Microsoft.UI.Xaml.Navigation;
using Shulkerbox.Models;

namespace Shulkerbox.Pages;

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