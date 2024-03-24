using System.Diagnostics;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Installer.Forge.Versions;
using CmlLib.Core.Installer.LiteLoader;
using CmlLib.Core.Installer.QuiltMC;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;
using CmlLib.Core.VersionMetadata;

namespace Shulkerbox;

public class ShulkLauncher
{
    private readonly CMLauncher _launcher = new(new MinecraftPath());
    private readonly MojangVersionLoader _vanillaLoader = new();
    private readonly ForgeVersionLoader _forgeLoader = new(new HttpClient());
    private readonly FabricVersionLoader _fabricLoader = new();
    private readonly QuiltVersionLoader _quiltLoader = new();
    private readonly LiteLoaderVersionLoader _liteLoader = new();

    private readonly ShulkLaunchEventArgs _currentEventArgs = new();

    public MinecraftPath Path => _launcher.MinecraftPath;

    public event EventHandler<ShulkConsoleEventArgs>? ConsoleUpdated;
    public event EventHandler<ShulkLaunchEventArgs>? LaunchUpdated;
    public event EventHandler? GameLaunched;
    public event EventHandler? GameExited;

    public ShulkLauncher()
    {
        _launcher.FileChanged += args =>
        {
            _currentEventArgs.FileName = args.FileName;
            _currentEventArgs.FileKind = args.FileKind;
            _currentEventArgs.ProgressedFileCount = args.ProgressedFileCount;
            _currentEventArgs.TotalFileCount = args.TotalFileCount;
            LaunchUpdated?.Invoke(this, _currentEventArgs);
        };
        _launcher.ProgressChanged += (_, args) =>
        {
            _currentEventArgs.OverallProgressPercentage = args.ProgressPercentage;
            LaunchUpdated?.Invoke(this, _currentEventArgs);
        };
    }

    public async Task<Process> StartAsync(MSession session, MVersion version, MLaunchOption? options = null)
    {
        options ??= new MLaunchOption();
        options.VersionType = "Shulkerbox";
        options.Session = session;
        var process = await _launcher.CreateProcessAsync(version.Id, options);
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.EnableRaisingEvents = true;
        process.Exited += (_, _) => GameExited?.Invoke(this, EventArgs.Empty);
#if !DEBUG
        process.Start();
        await Task.Run(() =>
        {
            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                ConsoleUpdated?.Invoke(this, new ShulkConsoleEventArgs { Message = line });
            }
        });
#endif
        GameLaunched?.Invoke(this, EventArgs.Empty);
        return process;
    }

    public async Task<MVersionMetadata[]> GetLocalVersionsAsync()
    {
        return (await _launcher.GetAllVersionsAsync())
            .Where(version => version.IsLocalVersion)
            .ToArray();
    }

    public async Task<MVersionCollection> GetVanillaVersionsAsync()
    {
        return await _vanillaLoader.GetVersionMetadatasAsync();
    }

    public async Task<ForgeVersion[]> GetForgeVersionsAsync(string versionName)
    {
        return (await _forgeLoader.GetForgeVersions(versionName)).ToArray();
    }

    public async Task<MVersionCollection> GetFabricVersionsAsync()
    {
        return await _fabricLoader.GetVersionMetadatasAsync();
    }

    public async Task<MVersionCollection> GetQuiltVersionsAsync()
    {
        return await _quiltLoader.GetVersionMetadatasAsync();
    }

    public async Task<MVersionCollection> GetLiteLoaderVersionsAsync()
    {
        return await _liteLoader.GetVersionMetadatasAsync();
    }
}