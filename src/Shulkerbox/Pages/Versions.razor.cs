using System.IO;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Installer.LiteLoader;
using CmlLib.Core.Installer.QuiltMC;
using CmlLib.Core.VersionLoader;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Models;
using Shulkerbox.Services;

namespace Shulkerbox.Pages;

public partial class Versions
{
    private readonly MojangVersionLoader _vanillaLoader = new();
    private readonly FabricVersionLoader _fabricLoader = new();
    private readonly QuiltVersionLoader _quiltLoader = new();
    private readonly LiteLoaderVersionLoader _liteLoader = new();

    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private GameService GameService { get; init; }

    private IList<VersionModel> GameVersions { get; } = new List<VersionModel>();

    private async Task ChangeType(string text)
    {
        GameVersions.Clear();
        switch (text)
        {
            case "Vanilla":
                foreach (var version in await _vanillaLoader.GetVersionMetadatasAsync())
                    GameVersions.Add(new VersionModel(version));
                break;
            case "Fabric":
                foreach (var version in await _fabricLoader.GetVersionMetadatasAsync())
                    GameVersions.Add(new VersionModel(version));
                break;
            case "Quilt":
                foreach (var version in await _quiltLoader.GetVersionMetadatasAsync())
                    GameVersions.Add(new VersionModel(version));
                break;
            case "LiteLoader":
                foreach (var version in await _liteLoader.GetVersionMetadatasAsync())
                    GameVersions.Add(new VersionModel(version));
                break;
            default:
                var versions = (await GameService.Launcher.GetAllVersionsAsync())
                    .Where(version => version.IsLocalVersion)
                    .Select(version => new VersionModel(version));
                foreach (var version in versions)
                    GameVersions.Add(version);
                break;
        }
    }

    public async Task SaveVersion(VersionModel version)
    {
        await version.Version.SaveAsync(GameService.Launcher.MinecraftPath);
        Snackbar.Add("The version has been saved!", Severity.Success);
    }

    public Task DeleteVersion(VersionModel version)
    {
        Directory.Delete(Path.Combine(GameService.Launcher.MinecraftPath.BasePath, "versions", version.Name), true);
        GameVersions.Remove(version);
        Snackbar.Add("The version has been deleted.", Severity.Info);
        return Task.CompletedTask;
    }
}