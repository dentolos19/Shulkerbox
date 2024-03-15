using System.Reflection;
using System.Text.Json;
using Shulkerbox.Shared.Models;

namespace Shulkerbox.Shared.Services;

public class SettingsService
{
    private static readonly string FilePath =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            $"{Assembly.GetExecutingAssembly().GetName().Name}.settings.json"
        );

    private static readonly JsonSerializerOptions SerializerOptions =
        new()
        {
            WriteIndented = true
        };

    public int MaximumMemoryAllocation { get; set; } = 1024 * 4;
    public int MinimumMemoryAllocation { get; set; } = 1024;
    public bool EnableFullScreen { get; set; }
    public IList<AccountModel> Accounts { get; set; } = new List<AccountModel>();
    public string? LastAccountUsed { get; set; }
    public string? LastVersionUsed { get; set; }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, SerializerOptions);
        File.WriteAllText(FilePath, json);
    }

    public static SettingsService Initialize()
    {
        if (!File.Exists(FilePath))
            return new SettingsService();
        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<SettingsService>(json)!;
    }
}