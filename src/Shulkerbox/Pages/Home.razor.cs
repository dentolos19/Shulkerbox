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

    private MSession? _currentSession;
    private MVersion? _currentVersion;
    private IList<string>? _gameLogs = new List<string>();
    private bool _isLaunching;
    private int? _launchProgress;
    private string? _launchStatus;


    protected override async Task OnInitializedAsync()
    {
        Launcher.ConsoleUpdated += (_, args) =>
        {
            // TODO
        };
        Launcher.LaunchUpdated += (_, args) =>
        {
            _isLaunching = true;
            _launchProgress = args.OverallProgressPercentage <= 0 ? null : args.OverallProgressPercentage;
            _launchStatus = string.Format(
                "[{0}] {1} - {2}/{3}",
                args.FileKind.ToString(),
                args.FileName,
                args.ProgressedFileCount,
                args.TotalFileCount
            );
            StateHasChanged();
        };
        Launcher.GameLaunched += (_, _) => _isLaunching = false;
        if (Settings.LastAccountUsed is not null)
        {
            var account = Settings.Accounts.FirstOrDefault(account => account.Id == Settings.LastAccountUsed);
            if (account is null)
                return;
            _currentSession = await account.CreateSessionAsync();
        }
        if (Settings.LastVersionUsed is not null)
        {
            var version =
                (await Launcher.GetLocalVersionsAsync()).FirstOrDefault(version =>
                    version.Name == Settings.LastVersionUsed);
            if (version is null)
                return;
            _currentVersion = await version.GetVersionAsync();
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
        if (_currentSession is null)
        {
            Snackbar.Add("Please select an account to use.", Severity.Error);
            return;
        }
        if (_currentVersion is null)
        {
            Snackbar.Add("Please select a version to use.", Severity.Error);
            return;
        }
        await Launcher.StartAsync(_currentSession, _currentVersion);
    }

    private Task OpenGameDirectory()
    {
        ShulkUtils.ExecuteShell(Launcher.Path.ToString());
        return Task.CompletedTask;
    }

    private async Task OpenLauncherSettings()
    {
        await DialogService.ShowAsync<Home_Settings>("Settings",
            new DialogOptions
            {
                MaxWidth = MaxWidth.ExtraLarge
            });
    }
}