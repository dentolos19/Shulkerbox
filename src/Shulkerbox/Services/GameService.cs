using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using Windows.Storage;
using CmlLib.Core;
using CmlLib.Core.VersionLoader;

namespace Shulkerbox.Services;

public class GameService
{
    private readonly HttpClient _httpClient;

    public static GameService Instance => App.GetService<GameService>();

    public MinecraftPath LauncherPath { get; }
    public CMLauncher Launcher { get; }

    public GameService()
    {
        _httpClient = new HttpClient();
        LauncherPath = new MinecraftPath(
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "game", "instances", "default"),
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "game", "assets")
        );
        Launcher = new CMLauncher(LauncherPath)
        {
            VersionLoader = new LocalVersionLoader(LauncherPath)
        };
    }

    public Uri GetHeadImageUrl(string username, int size = 128)
    {
        var headsDirectoryPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "heads");
        if (!Directory.Exists(headsDirectoryPath))
            Directory.CreateDirectory(headsDirectoryPath);
        var headFilePath = Path.Combine(headsDirectoryPath, $"{username}.png");
        if (!File.Exists(headFilePath))
        {
            var json = _httpClient
                .GetStringAsync($"https://minecraft-api.com/api/skins/{username}/head/0/0/{size}/json").Result;
            var data = JsonNode.Parse(json)["head"].ToString();
            var bytes = Convert.FromBase64String(data);
            File.WriteAllBytes(headFilePath, bytes);
        }
        return new Uri(headFilePath);
    }
}