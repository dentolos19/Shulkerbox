using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shulkerbox.Pages;

public partial class Accounts_AddOfflineDialog
{
    [CascadingParameter] private MudDialogInstance Instance { get; set; }

    private string Username { get; set; }

    private void Add()
    {
        Instance.Close(DialogResult.Ok(Username));
    }

    private void Cancel()
    {
        Instance.Cancel();
    }
}