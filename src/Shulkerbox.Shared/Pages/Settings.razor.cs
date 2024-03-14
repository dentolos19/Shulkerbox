using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared.Pages;

public partial class Settings
{
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private int MaximumMemoryAllocation { get; set; }
    private int MinimumMemoryAllocation { get; set; }
    private bool EnableFullScreen { get; set; }
    private string AboutText { get; set; }

    protected override async void OnInitialized()
    {
        MaximumMemoryAllocation = SettingsService.MaximumMemoryAllocation;
        MinimumMemoryAllocation = SettingsService.MinimumMemoryAllocation;
        EnableFullScreen = SettingsService.EnableFullScreen;
        AboutText = await Utilities.GetResourceStringAsync("About.md");
    }

    private void Save()
    {
        SettingsService.MaximumMemoryAllocation = MaximumMemoryAllocation;
        SettingsService.MinimumMemoryAllocation = MinimumMemoryAllocation;
        SettingsService.EnableFullScreen = EnableFullScreen;
        SettingsService.Save();
        Snackbar.Add("Your settings has been saved!", Severity.Info);
    }
}