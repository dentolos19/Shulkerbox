using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shulkerbox.Pages;

public partial class Home
{
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private IDialogService DialogService { get; init; }
    [Inject] private ISnackbar Snackbar { get; init; }
    [Inject] private ShulkLauncher Launcher { get; init; }
    [Inject] private ShulkSettings Settings { get; init; }

    private bool IsLaunching { get; set; }
    private int? LaunchProgress { get; set; }
    private string? LaunchStatus { get; set; }
    private MSession? CurrentSession { get; set; }
    private MVersion? CurrentVersion { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Launcher.ConsoleUpdated += (_, args) =>
        {
            // TODO: Implement console output here...
        };
        Launcher.LaunchUpdated += (_, args) =>
        {
            IsLaunching = true;
            LaunchProgress = args.OverallProgressPercentage <= 0 ? null : args.OverallProgressPercentage;
            LaunchStatus = string.Format(
                "[{0}] {1} - {2}/{3}",
                args.FileKind.ToString(),
                args.FileName,
                args.ProgressedFileCount,
                args.TotalFileCount
            );
            StateHasChanged();
        };
        Launcher.GameLaunched += (_, _) => IsLaunching = false;
        if (Settings.LastAccountUsed is not null)
        {
            var account = Settings.Accounts.FirstOrDefault(account => account.Id == Settings.LastAccountUsed);
            if (account is not null)
                CurrentSession = await account.CreateSessionAsync();
        }
        if (Settings.LastVersionUsed is not null)
        {
            var version = (await Launcher.GetLocalVersionsAsync()).FirstOrDefault(version => version.Name == Settings.LastVersionUsed);
            if (version is not null)
                CurrentVersion = await version.GetVersionAsync();
        }
    }

    private Task ShowAccounts()
    {
        NavigationManager.NavigateTo("/accounts");
        return Task.CompletedTask;
    }

    private Task ShowVersions()
    {
        NavigationManager.NavigateTo("/versions");
        return Task.CompletedTask;
    }

    private async Task LaunchGame()
    {
        if (CurrentSession is null)
        {
            Snackbar.Add("Please select an account to use.", Severity.Error);
            return;
        }
        if (CurrentVersion is null)
        {
            Snackbar.Add("Please select a version to use.", Severity.Error);
            return;
        }
        var options = new MLaunchOption
        {
            MaximumRamMb = Settings.MaximumMemoryAllocation,
            MinimumRamMb = Settings.MinimumMemoryAllocation
        };
        await Launcher.StartAsync(CurrentSession, CurrentVersion, options);
    }

    private Task OpenGameDirectory()
    {
        ShulkUtils.ExecuteShell(Launcher.Path.ToString());
        return Task.CompletedTask;
    }

    private async Task OpenLauncherSettings()
    {
        await DialogService.ShowAsync<Home_Settings>("Settings");
    }
}