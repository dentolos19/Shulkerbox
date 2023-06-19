using System.Text.RegularExpressions;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Models;
using Shulkerbox.Services;

namespace Shulkerbox.Pages;

public partial class Accounts
{
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private IList<AccountModel> UserAccounts { get; set; } = new List<AccountModel>();

    protected override void OnInitialized()
    {
        UserAccounts = SettingsService.Accounts;
    }

    private async Task AddMicrosoft()
    {
        var loginWindow = new MicrosoftLoginWindow();
        var session = await loginWindow.ShowLoginDialog();
        if (session is null)
            return;
        UserAccounts.Add(new AccountModel(session, "Premium"));
        Snackbar.Add("Account added!", Severity.Success);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
    }

    private async Task AddOffline()
    {
        var dialog = await DialogService.ShowAsync<Accounts_AddOfflineDialog>("Add Offline Account");
        var result = await dialog.Result;
        if (result.Canceled)
            return;
        if (!(result.Data is string username && Regex.IsMatch(username, "^[a-zA-Z0-9_]{2,16}$")))
        {
            Snackbar.Add("Invalid username!", Severity.Error);
            return;
        }
        if (UserAccounts.Any(account => account.Session.Username == username && account.Type == "Offline"))
        {
            Snackbar.Add("Account already exists!", Severity.Error);
            return;
        }
        UserAccounts.Add(new AccountModel(MSession.GetOfflineSession(username), "Offline"));
        Snackbar.Add("Account added!", Severity.Success);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
    }

    private void DeleteAccount(AccountModel account)
    {
        UserAccounts.Remove(account);
        Snackbar.Add("Account deleted!", Severity.Success);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
    }
}