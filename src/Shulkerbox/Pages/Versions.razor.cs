using CmlLib.Core.VersionMetadata;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shulkerbox.Pages;

public partial class Versions
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ShulkLauncher Launcher { get; init; }
    [Inject] private ShulkSettings Settings { get; init; }

    private bool IsLoading { get; set; }
    private string SearchQuery { get; set; }
    private IList<MVersionMetadata> Data { get; } = new List<MVersionMetadata>();

    private IList<MVersionMetadata> FilteredData => Data.Where(version =>
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
            return true;
        return version.Name.Contains(SearchQuery, StringComparison.InvariantCultureIgnoreCase);
    }).ToList();

    private async Task UpdateVersions(string? name = null)
    {
        if (IsLoading)
            return;
        IsLoading = true;
        Data.Clear();
        switch (name)
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
        IsLoading = false;
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

    private async Task DeleteVersion(MVersionMetadata version)
    {
        if (!version.IsLocalVersion || string.IsNullOrEmpty(version.Path))
            return;
        var actionConfirmed = await DialogService.ShowMessageBox(
            "Delete Version",
            "Are you sure you want to delete this version?",
            yesText: "Yes",
            cancelText: "No"
        );
        if (actionConfirmed != true)
            return;
        var path = Path.GetDirectoryName(version.Path);
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        await UpdateVersions();
    }
}