using System;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CmlLib.Core;

namespace Shulkerbox.Services;

public class GameService
{

    private readonly Regex _nameRegex = new("^[a-zA-Z0-9_]{2,16}$");
    private readonly HttpClient _httpClient = new();

    public CMLauncher Launcher { get; } = new(new MinecraftPath());

    public bool CheckUsername(string username)
    {
        return _nameRegex.Match(username).Success;
    }

    public async Task<Uri> GetHeadAsync(string username, int size = 128)
    {
        var headsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "heads");
        if (!Directory.Exists(headsDirectoryPath))
            Directory.CreateDirectory(headsDirectoryPath);
        var headFilePath = Path.Combine(headsDirectoryPath, $"{username}.png");
        if (!File.Exists(headFilePath))
        {
            var json = await _httpClient.GetStringAsync($"https://minecraft-api.com/api/skins/{username}/head/0/0/{size}/json");
            var data = JsonNode.Parse(json)["head"].ToString();
            var bytes = Convert.FromBase64String(data);
            await File.WriteAllBytesAsync(headFilePath, bytes);
        }
        return new Uri(headFilePath);
    }

}