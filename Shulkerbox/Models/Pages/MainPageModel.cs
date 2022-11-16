using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shulkerbox.Pages;
using Shulkerbox.Services;
using MessageBox = AdonisUI.Controls.MessageBox;
using MessageBoxButton = AdonisUI.Controls.MessageBoxButton;
using MessageBoxImage = AdonisUI.Controls.MessageBoxImage;

namespace Shulkerbox.Models;

public partial class MainPageModel : ObservableObject
{

    [ObservableProperty] private string _name;
    [ObservableProperty] private VersionItemModel? _version;
    [ObservableProperty] private ObservableCollection<VersionItemModel> _versions = new();
    [ObservableProperty] private Visibility _launchStatusVisibility = Visibility.Hidden;
    [ObservableProperty] private string _launchText;
    [ObservableProperty] private int _launchProgress;
    [ObservableProperty] private bool _isNotLaunching = true;

    private GameService Game => App.GetService<GameService>();
    private SettingsService Settings => App.GetService<SettingsService>();

    public MainPageModel()
    {
        Game.Launcher.FileChanged += args => LaunchText = string.Format(
            "[{0}] {1} - {2}/{3}",
            args.FileKind.ToString(),
            args.FileName,
            args.ProgressedFileCount,
            args.TotalFileCount
        );
        Game.Launcher.ProgressChanged += (_, args) => LaunchProgress = args.ProgressPercentage;
    }

    [RelayCommand]
    private async Task Launch()
    {
        if (!Game.CheckUsername(Name))
        {
            MessageBox.Show(
                "Please enter a valid username before launching.",
                "Shulkerbox",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }
        if (Version is null)
        {
            MessageBox.Show(
                "Please select a valid version before launching.",
                "Shulkerbox",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }
        Settings.LastUsedName = Name;
        Settings.LastUsedVersionName = Version.Name;
        Settings.Save();
        var options = new MLaunchOption
        {
            VersionType = "Shulkerbox",
            Session = MSession.GetOfflineSession(Settings.LastUsedName),
            MaximumRamMb = Settings.MemoryAllocation
        };
        LaunchStatusVisibility = Visibility.Visible;
        IsNotLaunching = false;
        var process = await Game.Launcher.CreateProcessAsync(Settings.LastUsedVersionName, options);
        IsNotLaunching = true;
        LaunchStatusVisibility = Visibility.Hidden;
        process.Start();
        RefreshCommand.Execute(null);
    }

    [RelayCommand]
    private void Refresh()
    {
        Versions.Clear();
        Version = null;
        foreach (var version in Game.Launcher.GetAllVersions())
            switch (version.MType)
            {
                case MVersionType.Release:
                case MVersionType.Custom:
                case MVersionType.Snapshot when Settings.ShowSnapshots:
                    Versions.Add(new VersionItemModel
                    {
                        Name = version.Name,
                        IsLocal = version.IsLocalVersion
                    });
                    break;
            }
        if (Settings.LastUsedVersionName is not null)
            Version = Versions.FirstOrDefault(
                version => version?.Name == Settings.LastUsedVersionName,
                Versions.FirstOrDefault()
            );
        else
            Version = Versions.FirstOrDefault();
        Name = Settings.LastUsedName ?? Environment.UserName;
    }

    [RelayCommand]
    private void OpenGameDirectory()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = Game.Launcher.MinecraftPath.BasePath,
            UseShellExecute = true
        });
    }

    [RelayCommand]
    private void OpenSettings()
    {
        App.Navigate(nameof(SettingsPage));
    }

}