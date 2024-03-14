using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Models;
using Shulkerbox.Shared.Services;

namespace Shulkerbox.Shared.Pages;

public partial class Index_EditLaunchSettings
{
    [Inject] private GameService GameService { get; set; }
    [Inject] private SettingsService SettingsService { get; set; }

    [CascadingParameter] private MudDialogInstance Instance { get; set; }

    private IList<AccountModel> UserAccounts { get; set; } = new List<AccountModel>();
    private IList<VersionModel> GameVersions { get; set; } = new List<VersionModel>();

    private AccountModel? Account { get; set; }
    private VersionModel? Version { get; set; }

    protected override async Task OnInitializedAsync()
    {
        UserAccounts = SettingsService.Accounts;
        GameVersions = (await GameService.Launcher.GetAllVersionsAsync())
            .Where(version => version.IsLocalVersion)
            .Select(version => new VersionModel(version)).ToList();
        if (!string.IsNullOrEmpty(SettingsService.LastAccountUsed))
            Account = UserAccounts.FirstOrDefault(account => account.Session.Username == SettingsService.LastAccountUsed);
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