using System.Diagnostics;
using CmlLib.Core;

namespace Shulkerbox.Shared.Services;

public class GameService
{
    public CMLauncher Launcher { get; } = new(new MinecraftPath());
    public Process? GameProcess { get; set; }

    public static GameService Initialize()
    {
        return new GameService();
    }
}