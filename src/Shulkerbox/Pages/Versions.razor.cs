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
    private MojangVersionLoader _vanillaLoader = new();
    private FabricVersionLoader _fabricLoader = new();
    private QuiltVersionLoader _quiltLoader = new();
    private LiteLoaderVersionLoader _liteLoader = new();

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
        }
    }

    public async Task SaveVersion(VersionModel version)
    {
        await version.Version.SaveAsync(GameService.Launcher.MinecraftPath);
        Snackbar.Add("Version saved!", Severity.Success);
    }
}