using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Objects;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared.Pages;

public partial class Accounts
{
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private AuthenticationService AuthenticationService { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private IList<MinecraftAccount> UserAccounts { get; set; } = new List<MinecraftAccount>();

    protected override void OnInitialized()
    {
        UserAccounts = SettingsService.Accounts;
    }

    private async Task AddMicrosoft()
    {
        var dialog = await DialogService.ShowAsync<Accounts_MicrosoftLoginDialog>("Add Microsoft Account");
        var result = await dialog.Result;
        if (result.Canceled)
            return;
        // TODO
    }

    private async Task AddOffline()
    {
        var dialog = await DialogService.ShowAsync<Accounts_AddOfflineDialog>("Add Offline Account");
        var result = await dialog.Result;
        if (result.Canceled)
            return;
        if (result.Data is not string username || !Regex.IsMatch(username, "^[a-zA-Z0-9_]{2,16}$"))
        {
            Snackbar.Add("You've entered an invalid username.", Severity.Error);
            return;
        }
        if (UserAccounts.Any(account =>
                account.Session.Username == username &&
                account.Type == MinecraftAccountType.Offline
            ))
        {
            Snackbar.Add("The account already exists.", Severity.Error);
            return;
        }
        var session = AuthenticationService.CreateOfflineAccount(username);
        UserAccounts.Add(new MinecraftAccount(session, MinecraftAccountType.Offline));
        Snackbar.Add("Your new account has been added!", Severity.Success);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
    }

    private async Task DeleteAccount(MinecraftAccount account)
    {
        if (await DialogService.ShowMessageBox(
                "Delete Account",
                "Are you sure you want to delete this account?",
                "Yes",
                cancelText: "No"
            ) !=
            true)
            return;
        UserAccounts.Remove(account);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
        Snackbar.Add("The account has been deleted.", Severity.Info);
    }
}