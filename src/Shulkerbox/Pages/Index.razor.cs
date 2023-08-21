using System.Diagnostics;
using CmlLib.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Models;
using Shulkerbox.Services;

namespace Shulkerbox.Pages;

public partial class Index
{
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private LayoutService LayoutService { get; init; }
    [Inject] private GameService GameService { get; init; }
    [Inject] private SettingsService SettingsService { get; init; }

    private AccountModel? CurrentAccount { get; set; }
    private VersionModel? CurrentVersion { get; set; }
    private bool IsLaunching { get; set; }
    private string LaunchStatus { get; set; }

    protected override async Task OnInitializedAsync()
    {
        GameService.Launcher.FileChanged += args =>
        {
            LaunchStatus = string.Format(
                "[{0}] {1} - {2}/{3}",
                args.FileKind.ToString(),
                args.FileName,
                args.ProgressedFileCount,
                args.TotalFileCount
            );
            StateHasChanged();
        };
        if (!string.IsNullOrEmpty(SettingsService.LastAccountUsed))
        {
            var accounts = SettingsService.Accounts;
            CurrentAccount = accounts.FirstOrDefault(account => account.Session.Username == SettingsService.LastAccountUsed);
        }
        if (!string.IsNullOrEmpty(SettingsService.LastVersionUsed))
        {
            var versions = (await GameService.Launcher.GetAllVersionsAsync())
                .Where(version => version.IsLocalVersion)
                .Select(version => new VersionModel(version)).ToList();
            CurrentVersion = versions.FirstOrDefault(version => version.Name == SettingsService.LastVersionUsed);
        }
    }

    private async Task LaunchGame()
    {
        if (CurrentAccount is null)
        {
            Snackbar.Add("Please select an account before launching.", Severity.Error);
            return;
        }
        if (CurrentVersion is null)
        {
            Snackbar.Add("Please select a version before launching.", Severity.Error);
            return;
        }
        IsLaunching = true;
        LayoutService.IsLockDownMode = true;
        LayoutService.TriggerStateChanged();
        var launchOptions = new MLaunchOption
        {
            VersionType = "Shulkerbox",
            Session = CurrentAccount.Session,
            MaximumRamMb = SettingsService.MaximumMemoryAllocation,
            MinimumRamMb = SettingsService.MinimumMemoryAllocation,
            FullScreen = SettingsService.EnableFullScreen
        };
        GameService.GameProcess = await GameService.Launcher.CreateProcessAsync(CurrentVersion.Name, launchOptions);
#if !DEBUG
        GameService.GameProcess.Start();
#endif
        IsLaunching = false;
        LayoutService.IsLockDownMode = false;
        LayoutService.TriggerStateChanged();
    }

    private Task OpenGameDirectory()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = GameService.Launcher.MinecraftPath.BasePath,
            UseShellExecute = true
        });
        return Task.CompletedTask;
    }

    private async Task EditLaunchSettings()
    {
        var dialog = await DialogService.ShowAsync<Index_EditLaunchSettings>("Edit Launch Settings");
        var result = await dialog.Result;
        if (result.Canceled)
            return;
        if (result.Data is not ValueTuple<AccountModel?, VersionModel?>(var account, var version))
            return;
        CurrentAccount = account;
        CurrentVersion = version;
        SettingsService.LastAccountUsed = account.Session.Username;
        SettingsService.LastVersionUsed = version.Name;
        SettingsService.Save();
    }
}