using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Objects;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared.Pages;

public partial class Index_EditLaunchSettings
{
    [Inject] private GameService GameService { get; set; }
    [Inject] private SettingsService SettingsService { get; set; }

    [CascadingParameter] private MudDialogInstance Instance { get; set; }

    private IList<MinecraftAccount> UserAccounts { get; set; } = new List<MinecraftAccount>();
    private IList<MinecraftVersion> GameVersions { get; set; } = new List<MinecraftVersion>();

    private MinecraftAccount? Account { get; set; }
    private MinecraftVersion? Version { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UserAccounts = SettingsService.Accounts;
        GameVersions = (await GameService.Launcher.GetAllVersionsAsync())
            .Where(version => version.IsLocalVersion)
            .Select(version => new MinecraftVersion(version))
            .ToList();
        if (!string.IsNullOrEmpty(SettingsService.LastAccountUsed))
            Account = UserAccounts.FirstOrDefault(
                account => account.Session.Username == SettingsService.LastAccountUsed);
        if (!string.IsNullOrEmpty(SettingsService.LastVersionUsed))
            Version = GameVersions.FirstOrDefault(version => version.Name == SettingsService.LastVersionUsed);
    }

    private void Save()
    {
        if (Account is null || Version is null)
            return;
        Instance.Close(DialogResult.Ok((Account, Version)));
    }

    private void Cancel()
    {
        Instance.Cancel();
    }
}