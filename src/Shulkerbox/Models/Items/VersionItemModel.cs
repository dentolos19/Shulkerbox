using System.Threading.Tasks;
using CmlLib.Core.VersionMetadata;
using CommunityToolkit.Mvvm.Input;
using Shulkerbox.Services;

namespace Shulkerbox.Models;

public partial class VersionItemModel
{
    private readonly MVersionMetadata _metadata;

    public string Name { get; }

    public VersionItemModel(MVersionMetadata version)
    {
        _metadata = version;
        Name = _metadata.Name;
    }

    [RelayCommand]
    private async Task Install()
    {
        await _metadata.SaveAsync(GameService.Instance.LauncherPath);
        await App.AttachDialog("This version has been saved.", "Done!");
    }
}