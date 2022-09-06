using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;
using static System.Net.Mime.MediaTypeNames;

namespace CraftMine.Models;

public partial class SettingsPageModel : ObservableObject
{

    private static readonly SettingsService _settingsService = App.GetService<SettingsService>();

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
        MemoryAllocation = _settingsService.MemoryAllocation;
        ShowSnapshots = _settingsService.ShowSnapshots;
        return Task.CompletedTask;
    }

    [RelayCommand]
    private Task Save()
    {
        _settingsService.MemoryAllocation = MemoryAllocation;
        _settingsService.ShowSnapshots = ShowSnapshots;
        return Task.CompletedTask;
    }

}