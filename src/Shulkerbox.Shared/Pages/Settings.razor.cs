using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared.Pages;

public partial class Settings
{
    // private readonly int _totalSystemMemory = Utilities.GetTotalSystemMemory();
    private readonly int _totalSystemMemory = 16 * 1024;

    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private ResourceService ResourceService { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private int MaximumMemoryAllocation { get; set; }
    private int MinimumMemoryAllocation { get; set; }
    private bool EnableFullScreen { get; set; }
    private string About { get; set; }

    protected override async void OnInitialized()
    {
        MaximumMemoryAllocation = SettingsService.MaximumMemoryAllocation;
        MinimumMemoryAllocation = SettingsService.MinimumMemoryAllocation;
        EnableFullScreen = SettingsService.EnableFullScreen;
        var text = await ResourceService.GetResourceStringAsync("About.md");
        About = text;
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