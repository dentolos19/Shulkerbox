using Microsoft.UI.Xaml.Navigation;
using Shulkerbox.Models;

namespace Shulkerbox.Pages;

public sealed partial class AccountsPage
{
    private AccountsPageModel Context => (AccountsPageModel)DataContext;

    public AccountsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs args)
    {
        Context.RefreshCommand.Execute(null);
    }
}