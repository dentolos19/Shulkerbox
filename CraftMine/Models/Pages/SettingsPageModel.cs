using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;

namespace CraftMine.Models;

public partial class SettingsPageModel : ObservableObject
{

    [ObservableProperty] private int _memoryAllocation;
    [ObservableProperty] private bool _showSnapshots;
    [ObservableProperty] private string _aboutText;

    public SettingsPageModel()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CraftMine.Resources.Raw.About.md");
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
        ShowSnapshots = SettingsService.Instance.ShowSnapshots;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task Save()
    {
        SettingsService.Instance.MemoryAllocation = MemoryAllocation;
        SettingsService.Instance.ShowSnapshots = ShowSnapshots;
        return Task.CompletedTask;
    }

}