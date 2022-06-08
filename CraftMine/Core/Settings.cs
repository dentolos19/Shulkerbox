using System;
using System.IO;
using System.Text.Json;

namespace CraftMine.Core;

public class Settings
{

    private static string FilePath = Path.Combine(App.Launcher.MinecraftPath.BasePath, "settings.json");

    public string Username { get; set; } = Environment.UserName;
    public int MemoryAllocation { get; set; } = 4096;
    public bool ShowSnapshots { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static Settings Load()
    {
        if (!File.Exists(FilePath))
            return new Settings();
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Settings>(json);
    }

}