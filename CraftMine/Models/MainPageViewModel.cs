using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using CmlLib.Core.Files;
using CmlLib.Core.Version;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Views.Dialogs;
using Microsoft.UI.Xaml;

namespace CraftMine.Models;

public partial class MainPageViewModel : ObservableObject
{

    [ObservableProperty] private string _title = Package.Current.DisplayName;
    [ObservableProperty] private string _username = App.Settings.Username;
    [ObservableProperty] private MinecraftVersionModel? _selectedVersion;
    [ObservableProperty] private ObservableCollection<MinecraftVersionModel> _versions = new();

    [ObservableProperty] private Visibility _launchVisibility = Visibility.Collapsed;
    [ObservableProperty] private string _launchMessage;
    [ObservableProperty] private int _launchProgress;

    [ObservableProperty] private bool _isNotLaunching = true;

    public MainPageViewModel()
    {
        App.Launcher.FileChanged += args =>
        {
            var message = args.Source switch
            {
                IFileChecker => "Checking",
                IDownloader => "Downloading",
                _ => "Launching"
            };
            if (!string.IsNullOrEmpty(args.FileName))
            {
                message += $": {args.FileName}";
                if (args.TotalFileCount > 0)
                    message += $" ({args.ProgressedFileCount}/{args.TotalFileCount})";
            }
            LaunchMessage = message;
        };
        App.Launcher.ProgressChanged += (_, args) => LaunchProgress = args.ProgressPercentage;
        _ = ReloadVersions();
    }

    private async Task ReloadVersions()
    {
        var versions = await App.Launcher.GetAllVersionsAsync();
        Versions.Clear();
        foreach (var version in versions)
            switch (version.MType)
            {
                case MVersionType.Release:
                case MVersionType.Snapshot when App.Settings.ShowSnapshots:
                    Versions.Add(new MinecraftVersionModel(version));
                    break;
            }
        SelectedVersion = Versions.First();
    }

    [ICommand]
    private async Task LaunchGame()
    {
        Username = Username.Trim();
        if (string.IsNullOrWhiteSpace(Username) || !Regex.IsMatch(Username, @"^\w+$"))
        {
            await App.AttachDialog("Enter a valid username!");
            return;
        }
        App.Settings.Username = Username;
        App.Settings.Save();
        if (SelectedVersion == null)
        {
            await App.AttachDialog("Select a valid Minecraft version!");
            return;
        }
        LaunchVisibility = Visibility.Visible;
        IsNotLaunching = false;
        var version = await SelectedVersion.Metadata.GetVersionAsync();
        var process = await App.Launcher.CreateProcessAsync(version, new MLaunchOption
        {
            MaximumRamMb = App.Settings.MemoryAllocation,
            Session = MSession.GetOfflineSession(Username),
            VersionType = Package.Current.DisplayName
        });
        process.Start();
        IsNotLaunching = true;
        LaunchVisibility = Visibility.Collapsed;
    }

    [ICommand]
    private async Task OpenFolder()
    {
        await Launcher.LaunchFolderPathAsync(App.Launcher.MinecraftPath.BasePath);
    }

    [ICommand]
    private async Task OpenSettings()
    {
        await App.AttachDialog(new SettingsDialog());
        await ReloadVersions();
    }

    [ICommand]
    private async Task OpenAbout()
    {
        await App.AttachDialog(new AboutDialog());
    }

}