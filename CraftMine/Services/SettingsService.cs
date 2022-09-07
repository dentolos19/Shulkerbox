using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CraftMine.Services;

public partial class SettingsService : ObservableObject
{

    public static SettingsService Instance => App.GetService<SettingsService>();

    private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

    [ObservableProperty] private string[] _accounts = { Environment.UserName };
    [ObservableProperty] private int _memoryAllocation = 2048;
    [ObservableProperty] private bool _showSnapshots;
    [ObservableProperty] private string? _lastAccountUsed;
    [ObservableProperty] private string? _lastVersionUsed;

    protected override void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        var json = JsonSerializer.Serialize(this);
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