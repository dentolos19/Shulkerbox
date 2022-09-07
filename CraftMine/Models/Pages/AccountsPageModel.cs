﻿using System;
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

    [ObservableProperty] private ObservableCollection<AccountItemModel> _accounts = new();

    public AccountsPageModel()
    {
        RefreshCommand.Execute(null);
    }

    [RelayCommand]
    private async Task Add(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            await App.AttachDialog("Enter a username before adding.", "Halt!");
            return;
        }
        if (SettingsService.Instance.Accounts.Contains(username, StringComparer.OrdinalIgnoreCase))
        {
            await App.AttachDialog("Your username already exists.", "Halt!");
            return;
        }
        if (!Regex.IsMatch(username, "^[a-zA-Z0-9_]{2,16}$"))
        {
            await App.AttachDialog("Your username is invalid.", "Halt!");
            return;
        }
        var accounts = SettingsService.Instance.Accounts.ToList();
        accounts.Add(username);
        SettingsService.Instance.Accounts = accounts.ToArray();
        await RefreshCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task Remove(AccountItemModel item)
    {
        if (Accounts.Count <= 1)
        {
            await App.AttachDialog("You must at least have one account available, therefore you can't remove this account.", "Halt!");
            return;
        }
        if (SettingsService.Instance.Accounts.Contains(item.Username, StringComparer.OrdinalIgnoreCase))
        {
            var accounts = SettingsService.Instance.Accounts.ToList();
            accounts.Remove(item.Username);
            SettingsService.Instance.Accounts = accounts.ToArray();
        }
        await RefreshCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task Refresh()
    {
        Accounts.Clear();
        var accounts = await Task.Run(() => SettingsService.Instance.Accounts.Select(account => new AccountItemModel(account)));
        Accounts = new ObservableCollection<AccountItemModel>(accounts);
    }

}