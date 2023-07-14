using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Shulkerbox.Services;

namespace Shulkerbox.Pages;

public partial class Settings
{
    [Inject] private SettingsService SettingsService { get; init; }

    private int MemoryAllocation { get; set; } = 2048;
    private string About { get; set; }

    protected override async Task OnInitializedAsync()
    {
        MemoryAllocation = SettingsService.MemoryAllocation;
        await using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Shulkerbox.Resources.About.md");
        using var reader = new StreamReader(stream);
        About = await reader.ReadToEndAsync();
    }
}