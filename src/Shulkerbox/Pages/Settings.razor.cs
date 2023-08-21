using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Services;

namespace Shulkerbox.Pages;

public partial class Settings
{
    private readonly int _totalSystemMemory = Utilities.GetTotalSystemMemory();

    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private int MaximumMemoryAllocation { get; set; }
    private int MinimumMemoryAllocation { get; set; }
    private bool EnableFullScreen { get; set; }
    private string About { get; set; }

    protected override async Task OnInitializedAsync()
    {
        MaximumMemoryAllocation = SettingsService.MaximumMemoryAllocation;
        MinimumMemoryAllocation = SettingsService.MinimumMemoryAllocation;
        EnableFullScreen = SettingsService.EnableFullScreen;
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Shulkerbox.Resources.About.md");
        using var reader = new StreamReader(stream!);
        About = await reader.ReadToEndAsync();
    }

    private Task Save()
    {
        SettingsService.MaximumMemoryAllocation = MaximumMemoryAllocation;
        SettingsService.MinimumMemoryAllocation = MinimumMemoryAllocation;
        SettingsService.EnableFullScreen = EnableFullScreen;
        SettingsService.Save();
        Snackbar.Add("Your settings has been saved!", Severity.Info);
        return Task.CompletedTask;
    }
}