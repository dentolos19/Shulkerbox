using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CraftMine.Models;

public partial class SettingsDialogViewModel : ObservableObject
{

    public int MemoryAllocation
    {
        get => App.Settings.MemoryAllocation;
        set
        {
            App.Settings.MemoryAllocation = value;
            OnPropertyChanged(nameof(MemoryAllocation));
        }
    }

    public bool ShowSnapshots
    {
        get => App.Settings.ShowSnapshots;
        set
        {
            App.Settings.ShowSnapshots = value;
            OnPropertyChanged(nameof(ShowSnapshots));
        }
    }

    [ICommand]
    private void Save()
    {
        App.Settings.Save();
    }

}