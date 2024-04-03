using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shulkerbox.Pages;

public partial class Accounts
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private ShulkSettings Settings { get; init; }

    private IList<ShulkAccount> Data { get; } = new List<ShulkAccount>();

    protected override void OnInitialized()
    {
        ReloadAccounts();
    }

    private void ReloadAccounts()
    {
        Data.Clear();
        foreach (var account in Settings.Accounts)
            Data.Add(account);
    }

    private async Task LoginMicrosoftAccount()
    {
        var dialog = await DialogService.ShowAsync<Accounts_LoginMicrosoftAccount>("Login With Microsoft");
        var result = await dialog.Result;
        if (result.Canceled)
            return;
        // TODO: Implement Microsoft authentication here...
    }

    private async Task AddOfflineAccount()
    {
        var dialog = await DialogService.ShowAsync<Accounts_AddOfflineAccount>("Add Offline Account");
        var result = await dialog.Result;
        if (result.Canceled || result.Data is not string username)
            return;
        if (!ShulkAuthenticator.ValidateUsername(username))
        {
            Snackbar.Add("Your username is invalid.", Severity.Error);
            return;
        }
        Settings.Accounts.Add(new ShulkAccount
        {
            Username = username,
            Type = ShulkAccountType.Offline
        });
        Settings.Save();
        ReloadAccounts();
    }

    private Task SelectAccount(ShulkAccount account)
    {
        Settings.LastAccountUsed = account.Id;
        Settings.Save();
        NavigationManager.NavigateTo("/");
        return Task.CompletedTask;
    }

    private async Task DeleteAccount(ShulkAccount account)
    {
        var actionConfirmed = await DialogService.ShowMessageBox(
            "Delete Account",
            "Are you sure you want to delete this account?",
            yesText: "Yes",
            cancelText: "No"
        );
        if (actionConfirmed != true)
            return;
        Settings.Accounts.Remove(account);
        Settings.Save();
        ReloadAccounts();
    }
}