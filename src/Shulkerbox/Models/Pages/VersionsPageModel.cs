using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;
using CmlLib.Core.VersionMetadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shulkerbox.Models;

public partial class VersionsPageModel : ObservableObject
{
    private readonly MojangVersionLoader _mojangLoader = new();
    private readonly FabricVersionLoader _fabricLoader = new();

    [ObservableProperty] private bool _loadReleaseVersions = true;
    [ObservableProperty] private bool _loadSnapshotVersions;
    [ObservableProperty] private bool _loadFabricVersions;
    [ObservableProperty] private ObservableCollection<VersionItemModel> _versions = new();

    [RelayCommand]
    private async Task Refresh()
    {
        var versions = new MVersionCollection(Array.Empty<MVersionMetadata>());
        if (LoadReleaseVersions || LoadSnapshotVersions)
            versions.Merge(await _mojangLoader.GetVersionMetadatasAsync());
        if (LoadFabricVersions)
            versions.Merge(await _fabricLoader.GetVersionMetadatasAsync());
        Versions.Clear();
        foreach (var version in versions)
            switch (version.MType)
            {
                case MVersionType.Release when LoadReleaseVersions:
                case MVersionType.Snapshot when LoadSnapshotVersions:
                case MVersionType.Custom when LoadFabricVersions:
                    Versions.Add(new VersionItemModel(version));
                    break;
            }
    }
}