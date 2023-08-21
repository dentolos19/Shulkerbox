using System.Text.RegularExpressions;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
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
        // TODO: create own auth with Microsoft
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var session = await loginHandler.Authenticate();
        UserAccounts.Add(new AccountModel(session, "Premium"));
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
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
        if (UserAccounts.Any(account => account.Session.Username == username && account.Type == "Offline"))
        {
            Snackbar.Add("The account already exists.", Severity.Error);
            return;
        }
        var session = MSession.GetOfflineSession(username);
        UserAccounts.Add(new AccountModel(session, "Offline"));
        Snackbar.Add("Your new account has been added!", Severity.Success);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
    }

    private void DeleteAccount(AccountModel account)
    {
        UserAccounts.Remove(account);
        SettingsService.Accounts = UserAccounts;
        SettingsService.Save();
        Snackbar.Add("The account has been deleted.", Severity.Info);
    }
}