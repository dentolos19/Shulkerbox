using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CraftMine.Services;

namespace CraftMine.Models;

public partial class SettingsDialogModel : ObservableObject
{

    private static readonly SettingsService _settingsService = App.GetService<SettingsService>();

    [ObservableProperty] private int _memoryAllocation;
    [ObservableProperty] private bool _showSnapshots;

    public SettingsDialogModel()
    {
        MemoryAllocation = _settingsService.MemoryAllocation;
        ShowSnapshots = _settingsService.ShowSnapshots;
    }

    [RelayCommand]
    private Task Save()
    {
        _settingsService.MemoryAllocation = MemoryAllocation;
        _settingsService.ShowSnapshots = ShowSnapshots;
        return Task.CompletedTask;
    }

}