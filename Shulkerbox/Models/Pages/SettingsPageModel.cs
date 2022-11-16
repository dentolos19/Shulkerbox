using AdonisUI.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shulkerbox.Services;

namespace Shulkerbox.Models;

public partial class SettingsPageModel : ObservableObject
{

    [ObservableProperty] private string _memoryAllocation;
    [ObservableProperty] private bool _showSnapshots;

    private SettingsService Settings => App.GetService<SettingsService>();

    [RelayCommand]
    private void Save()
    {
        var memoryAllocationValid = int.TryParse(MemoryAllocation, out var memoryAllocation);
        if (!memoryAllocationValid)
        {
            MessageBox.Show(
                "Your memory allocation is invalid!",
                "Shulkerbox",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
            return;
        }
        Settings.MemoryAllocation = memoryAllocation;
        Settings.ShowSnapshots = ShowSnapshots;
        Settings.Save();
        MessageBox.Show(
            "Your settings has been saved!",
            "Shulkerbox",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    [RelayCommand]
    private void Refresh()
    {
        MemoryAllocation = Settings.MemoryAllocation.ToString();
        ShowSnapshots = Settings.ShowSnapshots;
    }

    [RelayCommand]
    private void Back()
    {
        App.NavigateBack();
    }

}