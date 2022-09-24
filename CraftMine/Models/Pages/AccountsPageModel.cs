using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;

namespace CraftMine.Models;

public partial class AccountsPageModel : ObservableObject
{

    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private AccountItemModel? _account;
    [ObservableProperty] private ObservableCollection<AccountItemModel> _accounts = new();

    [RelayCommand]
    private async Task Add()
    {
        if (!Regex.IsMatch(Username, "^[a-zA-Z0-9_]{2,16}$"))
        {
            await App.AttachDialog("Your username is invalid.", "Halt!");
            return;
        }
        var accounts = new List<string>();
        if (SettingsService.Instance.Accounts is { Length: > 0 })
            accounts = SettingsService.Instance.Accounts.ToList();
        if (accounts.Contains(Username, StringComparer.OrdinalIgnoreCase))
        {
            await App.AttachDialog("Your username already exists.", "Halt!");
            return;
        }
        accounts.Add(Username);
        SettingsService.Instance.Accounts = accounts.ToArray();
        await RefreshCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task Remove()
    {
        if (Account is null)
        {
            await App.AttachDialog("Select an account to remove.", "Halt!");
            return;
        }
        if (SettingsService.Instance.Accounts.Contains(Account.Username, StringComparer.OrdinalIgnoreCase))
        {
            var accounts = SettingsService.Instance.Accounts.ToList();
            accounts.Remove(Account.Username);
            SettingsService.Instance.Accounts = accounts.ToArray();
        }
        await RefreshCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        Accounts.Clear();
        var accounts = await Task.Run(() => SettingsService.Instance.Accounts?.Select(account => new AccountItemModel(account)));
        Accounts = new ObservableCollection<AccountItemModel>(accounts ?? Array.Empty<AccountItemModel>());
    }

}