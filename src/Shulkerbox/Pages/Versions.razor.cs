using CmlLib.Core.VersionMetadata;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shulkerbox.Pages;

public partial class Versions
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private ShulkLauncher Launcher { get; init; }
    [Inject] private ShulkSettings Settings { get; init; }

    private IList<MVersionMetadata> Data { get; } = new List<MVersionMetadata>();

    private async Task UpdateVersions(MudChip chip)
    {
        Data.Clear();
        switch (chip.Text)
        {
            case "Vanilla":
                foreach (var version in await Launcher.GetVanillaVersionsAsync())
                    Data.Add(version);
                break;
            case "Fabric":
                foreach (var version in await Launcher.GetFabricVersionsAsync())
                    Data.Add(version);
                break;
            case "Quilt":
                foreach (var version in await Launcher.GetQuiltVersionsAsync())
                    Data.Add(version);
                break;
            case "LiteLoader":
                foreach (var version in await Launcher.GetLiteLoaderVersionsAsync())
                    Data.Add(version);
                break;
            default:
                foreach (var version in await Launcher.GetLocalVersionsAsync())
                    Data.Add(version);
                break;
        }
    }

    private Task SelectVersion(MVersionMetadata version)
    {
        if (!version.IsLocalVersion)
            version.Save(Launcher.Path);
        Settings.LastVersionUsed = version.Name;
        Settings.Save();
        NavigationManager.NavigateTo("/");
        return Task.CompletedTask;
    }
}