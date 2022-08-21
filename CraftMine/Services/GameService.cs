using CmlLib.Core;

namespace CraftMine.Services;

public class GameService
{

    public CMLauncher Launcher { get; }

    public GameService()
    {
        Launcher = new CMLauncher(new MinecraftPath());
    }

}