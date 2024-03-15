using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Msal;

namespace Shulkerbox.Shared.Services;

public sealed class AuthenticationService
{
    private readonly string _appClientId = "b7d86c5d-abc0-4002-9eca-a291cce6a053";

    public async Task<MSession> MsAuthViaInteractive()
    {
        var app = await MsalClientHelper.BuildApplicationWithCache(_appClientId);
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.Interactive());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public async Task<MSession> MsAuthViaDeviceCode(Action<DeviceCodeResult> callback)
    {
        var app = await MsalClientHelper.BuildApplicationWithCache(_appClientId);
        var loginHandler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = loginHandler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.DeviceCode(deviceCode =>
        {
            callback(deviceCode);
            return Task.CompletedTask;
        }));
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public MSession CreateOfflineAccount(string username)
    {
        return MSession.CreateOfflineSession(username);
    }

    public static AuthenticationService Initialize()
    {
        return new AuthenticationService();
    }
}