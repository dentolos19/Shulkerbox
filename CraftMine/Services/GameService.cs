using CmlLib.Core;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Windows.Storage;

namespace CraftMine.Services;

public class GameService
{

    public static GameService Instance => App.GetService<GameService>();

    private readonly HttpClient _httpClient;

    public CMLauncher Launcher { get; }

    public GameService()
    {
        _httpClient = new HttpClient();
        Launcher = new CMLauncher(new MinecraftPath());
    }

    public async Task<Uri> GetHeadImageUrl(string username)
    {
        var headsDirectoryPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Resources", "Heads");
        if (!Directory.Exists(headsDirectoryPath))
            Directory.CreateDirectory(headsDirectoryPath);
        var headFilePath = Path.Combine(headsDirectoryPath, $"{username}.png");
        if (!File.Exists(headFilePath))
        {
            var json = await _httpClient.GetStringAsync($"https://minecraft-api.com/api/skins/{username}/head/0/0/json");
            var data = JsonNode.Parse(json)["head"].ToString();
            var bytes = Convert.FromBase64String(data);
            await File.WriteAllBytesAsync(headFilePath, bytes);
        }
        return new Uri(headFilePath);
    }

}