using CmlLib.Core.Version;
using Microsoft.UI.Xaml.Controls;

namespace CraftMine.Models;

public class GameVersionItemModel
{

    public Symbol Icon { get; }
    public string Name { get; }

    public GameVersionItemModel(MVersionMetadata version)
    {
        Icon = version.IsLocalVersion ? Symbol.SaveLocal : Symbol.Globe;
        Name = version.Name;
    }

}