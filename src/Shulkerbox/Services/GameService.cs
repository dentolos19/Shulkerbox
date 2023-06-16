using System.Diagnostics;
using CmlLib.Core;

namespace Shulkerbox.Services;

public class GameService
{
    public CMLauncher Launcher { get; }
    public Process? GameProcess { get; set; }

    public GameService()
    {
        Launcher = new CMLauncher(new MinecraftPath());
    }
}