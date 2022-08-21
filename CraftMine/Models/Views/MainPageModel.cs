using System;
using System.Collections.Generic;
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
using CraftMine.Views;

namespace CraftMine.Models;

public partial class MainPageModel : ObservableObject
{

    private readonly GameService _gameService = App.GetService<GameService>();
    private readonly SettingsService _settingsService = App.GetService<SettingsService>();

    [ObservableProperty] private string _username;
    [ObservableProperty] private GameVersionItemModel _version;
    [ObservableProperty] private ObservableCollection<GameVersionItemModel> _versions = new();
    [ObservableProperty] private string _statusText;
    [ObservableProperty] private int _statusProgress;
    [ObservableProperty] private bool _isStatusVisible;

    public MainPageModel()
    {
        _gameService.Launcher.FileChanged += args => StatusText = string.Format(
            "[{0}] {1} - {2}/{3}",
            args.FileKind.ToString(),
            args.FileName,
            args.ProgressedFileCount,
            args.TotalFileCount
        );
        _gameService.Launcher.ProgressChanged += (_, args) => StatusProgress = args.ProgressPercentage;
        ReloadVersionsCommand.Execute(null);
        Username = _settingsService.Username;
    }

    [RelayCommand]
    private async Task ReloadVersions()
    {
        var versions = await _gameService.Launcher.GetAllVersionsAsync();
        var versionModels = new List<GameVersionItemModel>();
        foreach (var version in versions)
            switch (version.MType)
            {
                case MVersionType.Release:
                case MVersionType.Custom:
                case MVersionType.Snapshot when _settingsService.ShowSnapshots:
                    versionModels.Add(new GameVersionItemModel(version));
                    break;
            }
        Versions = new ObservableCollection<GameVersionItemModel>(versionModels);
        Version = (
            from version in Versions
            where version.Name == _settingsService.LastVersionUsed
            select version
        ).FirstOrDefault(Versions.First());
    }

    [RelayCommand]
    private async Task LaunchGame()
    {
        Username = Username.Trim();
        if (string.IsNullOrEmpty(Username))
        {
            await App.AttachDialog("Please enter a valid username.", "Unable to launch!");
            return;
        }
        _settingsService.Username = Username;
        _settingsService.LastVersionUsed = Version.Name;
        var launchOptions = new MLaunchOption
        {
            Session = MSession.GetOfflineSession(_settingsService.Username),
            MaximumRamMb = _settingsService.MemoryAllocation
        };
        IsStatusVisible = true;
        var gameProcess = await _gameService.Launcher.CreateProcessAsync(Version.Name, launchOptions);
        IsStatusVisible = false;
        gameProcess.Start();
        await ReloadVersionsCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task OpenGameDirectory()
    {
        await Launcher.LaunchFolderPathAsync(_gameService.Launcher.MinecraftPath.BasePath);
    }

    [RelayCommand]
    private async Task OpenAppSettings()
    {
        await App.AttachDialog(new SettingsDialog());
        await ReloadVersionsCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private async Task AboutThisApp()
    {
        await App.AttachDialog("Please use the latest version if possible.", "Work-in-progress!");
    }

}