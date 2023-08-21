using System.Diagnostics;
using CmlLib.Core;

namespace Shulkerbox.Services;

public class GameService
{
    public CMLauncher Launcher { get; } = new(new MinecraftPath());
    public Process? GameProcess { get; set; }
}