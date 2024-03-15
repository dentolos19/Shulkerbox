using CmlLib.Core.Auth.Microsoft;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using XboxAuthNet.Game.Msal;

namespace Shulkerbox.Shared.Pages;

public partial class Accounts_MicrosoftLoginDialog
{
    [CascadingParameter] private MudDialogInstance Instance { get; set; }

    private string Message { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var app = await MsalClientHelper.BuildApplicationWithCache("b7d86c5d-abc0-4002-9eca-a291cce6a053");
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(deviceCode =>
        {
            Console.WriteLine(deviceCode.Message);
            Message = deviceCode.Message;
            InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
        }));
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        Console.WriteLine("Authenticating...");
        var session = await authenticator.ExecuteForLauncherAsync();
        Console.WriteLine(session.Username);
        Console.WriteLine(session.UUID);
    }
}