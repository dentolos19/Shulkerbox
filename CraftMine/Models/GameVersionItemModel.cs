using CmlLib.Core.Version;

namespace CraftMine.Models;

public class GameVersionItemModel
{

    public string Name { get; }

    public GameVersionItemModel(MVersionMetadata version)
    {
        Name = version.Name;
    }

}