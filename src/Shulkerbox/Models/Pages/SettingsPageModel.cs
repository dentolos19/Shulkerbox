using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shulkerbox.Services;

namespace Shulkerbox.Models;

public partial class SettingsPageModel : ObservableObject
{
    [ObservableProperty] private int _memoryAllocation;
    [ObservableProperty] private string _aboutText;

    public SettingsPageModel()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Shulkerbox.Resources.Raw.About.md");
        if (stream is null)
        {
            AboutText = "Unable to load document.";
        }
        else
        {
            using var streamReader = new StreamReader(stream);
            AboutText = streamReader.ReadToEnd();
        }
        LoadCommand.Execute(null);
    }

    [RelayCommand]
    private Task Load()
    {
        MemoryAllocation = SettingsService.Instance.MemoryAllocation;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task Save()
    {
        SettingsService.Instance.MemoryAllocation = MemoryAllocation;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task OpenLauncherDirectory()
    {
        await Launcher.LaunchFolderAsync(ApplicationData.Current.LocalFolder);
    }

    [RelayCommand]
    private async Task OpenGameDirectory()
    {
        await Launcher.LaunchFolderPathAsync(GameService.Instance.Launcher.MinecraftPath.BasePath);
    }
}