using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Shulkerbox.Services;

public class SettingsService : ObservableObject
{

    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

    public string? LastUsedName { get; set; }
    public string? LastUsedVersionName { get; set; }
    public int MemoryAllocation { get; set; } = 4096;
    public bool ShowSnapshots { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public static SettingsService Initialize()
    {
        if (!File.Exists(FilePath))
            return new SettingsService();
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<SettingsService>(json);
    }

}