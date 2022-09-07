using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;

namespace CraftMine.Models;

public partial class HomePageModel : ObservableObject
{

    private readonly GameService _gameService = App.GetService<GameService>();
    private readonly SettingsService _settingsService = App.GetService<SettingsService>();

    [ObservableProperty] private AccountItemModel _account;
    [ObservableProperty] private ObservableCollection<AccountItemModel> _accounts = new();
    [ObservableProperty] private VersionItemModel _version;
    [ObservableProperty] private ObservableCollection<VersionItemModel> _versions = new();
    [ObservableProperty] private string _statusText;
    [ObservableProperty] private int _statusProgress;
    [ObservableProperty] private bool _isStatusVisible;

    public HomePageModel()
    {
        _gameService.Launcher.FileChanged += args => StatusText = string.Format(
            "[{0}] {1} - {2}/{3}",
            args.FileKind.ToString(),
            args.FileName,
            args.ProgressedFileCount,
            args.TotalFileCount
        );
        _gameService.Launcher.ProgressChanged += (_, args) => StatusProgress = args.ProgressPercentage;
        ReloadCommand.Execute(null);
    }

    [RelayCommand]
    private async Task Reload()
    {
        Accounts.Clear();
        foreach (var account in _settingsService.Accounts)
            Accounts.Add(new AccountItemModel(account));
        Account = (
            from account in Accounts
            where account.Username == _settingsService.LastAccountUsed
            select account
        ).FirstOrDefault(Accounts.First());
        Versions.Clear();
        var versions = await _gameService.Launcher.GetAllVersionsAsync();
        foreach (var version in versions)
            switch (version.MType)
            {
                case MVersionType.Release:
                case MVersionType.Custom:
                case MVersionType.Snapshot when _settingsService.ShowSnapshots:
                    Versions.Add(new VersionItemModel(version));
                    break;
            }
        Version = (
            from version in Versions
            where version.Name == _settingsService.LastVersionUsed
            select version
        ).FirstOrDefault(Versions.First());
    }

    [RelayCommand]
    private async Task Launch()
    {
        _settingsService.LastAccountUsed = Account.Username;
        _settingsService.LastVersionUsed = Version.Name;
        var launchOptions = new MLaunchOption
        {
            Session = MSession.GetOfflineSession(_settingsService.LastAccountUsed),
            MaximumRamMb = _settingsService.MemoryAllocation
        };
        IsStatusVisible = true;
        var gameProcess = await _gameService.Launcher.CreateProcessAsync(_settingsService.LastVersionUsed, launchOptions);
        IsStatusVisible = false;
        gameProcess.Start();
        await ReloadCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task OpenGameDirectory()
    {
        await Launcher.LaunchFolderPathAsync(_gameService.Launcher.MinecraftPath.BasePath);
    }

}