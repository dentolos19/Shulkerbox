using System.Text.Json;

namespace Shulkerbox;

public class ShulkSettings
{
    private static readonly string SettingsPath = Path.Combine(AppContext.BaseDirectory, "settings.json");
    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    public IList<ShulkAccount> Accounts { get; set; } = new List<ShulkAccount>();
    public Guid? LastAccountUsed { get; set; }
    public string? LastVersionUsed { get; set; }
    public int MinimumMemoryAllocation { get; set; } = 1024;
    public int MaximumMemoryAllocation { get; set; } = 4096;

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, SerializerOptions);
        File.WriteAllText(SettingsPath, json);
    }

    public static ShulkSettings Load()
    {
        if (!File.Exists(SettingsPath))
            return new ShulkSettings();
        try
        {
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<ShulkSettings>(json)!;
        }
        catch
        {
            return new ShulkSettings();
        }
    }
}