using CmlLib.Core.Auth.Microsoft;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shulkerbox.Shared.Services;
using XboxAuthNet.Game.Msal;

namespace Shulkerbox.Shared.Pages;

public partial class Accounts_MicrosoftLoginDialog
{
    [Inject] private AuthenticationService AuthenticationService { get; init; }

    [CascadingParameter] private MudDialogInstance Instance { get; set; }

    private string Message { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("Authenticating...");
        var session = await AuthenticationService.MsAuthViaDeviceCode(deviceCode =>
        {
            Message = deviceCode.Message;
            InvokeAsync(StateHasChanged);
        });
        Console.WriteLine(session.Username);
    }
}