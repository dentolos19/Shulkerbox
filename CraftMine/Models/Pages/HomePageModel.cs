using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;

namespace CraftMine.Models;

public partial class HomePageModel : ObservableObject
{

    [ObservableProperty] private AccountItemModel? _account;
    [ObservableProperty] private ObservableCollection<AccountItemModel> _accounts = new();
    [ObservableProperty] private VersionItemModel? _version;
    [ObservableProperty] private ObservableCollection<VersionItemModel> _versions = new();
    [ObservableProperty] private string _statusText;
    [ObservableProperty] private int _statusProgress;
    [ObservableProperty] private bool _isStatusVisible;

    public HomePageModel()
    {
        GameService.Instance.Launcher.FileChanged += args => StatusText = string.Format(
            "[{0}] {1} - {2}/{3}",
            args.FileKind.ToString(),
            args.FileName,
            args.ProgressedFileCount,
            args.TotalFileCount
        );
        GameService.Instance.Launcher.ProgressChanged += (_, args) => StatusProgress = args.ProgressPercentage;
        ReloadCommand.Execute(null);
    }

    [RelayCommand]
    private async Task Reload()
    {
        Accounts.Clear();
        var accounts = await Task.Run(() => SettingsService.Instance.Accounts?.Select(account => new AccountItemModel(account)));
        Accounts = new ObservableCollection<AccountItemModel>(accounts ?? Array.Empty<AccountItemModel>());
        Account = Accounts.FirstOrDefault(item => item.Username == SettingsService.Instance.LastAccountUsed);
        Versions.Clear();
        var versions = await GameService.Instance.Launcher.GetAllVersionsAsync();
        foreach (var version in versions)
            Versions.Add(new VersionItemModel(version));
        Version = Versions.FirstOrDefault(item => item.Name == SettingsService.Instance.LastVersionUsed);
    }

    [RelayCommand]
    private async Task Launch()
    {
        if (Account is null)
        {
            await App.AttachDialog("Please select an account before launching.", "Halt!");
            return;
        }
        if (Version is null)
        {
            await App.AttachDialog("Please select a version before launching.", "Halt!");
            return;
        }
        SettingsService.Instance.LastAccountUsed = Account.Username;
        SettingsService.Instance.LastVersionUsed = Version.Name;
        var launchOptions = new MLaunchOption
        {
            VersionType = "CraftMine",
            Session = MSession.GetOfflineSession(SettingsService.Instance.LastAccountUsed),
            MaximumRamMb = SettingsService.Instance.MemoryAllocation
        };
        IsStatusVisible = true;
        var gameProcess = await GameService.Instance.Launcher.CreateProcessAsync(SettingsService.Instance.LastVersionUsed, launchOptions);
        IsStatusVisible = false;
        gameProcess.Start();
        await ReloadCommand.ExecuteAsync(null);
    }

}