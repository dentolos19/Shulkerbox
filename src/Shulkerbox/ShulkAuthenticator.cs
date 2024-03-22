using System.Text.RegularExpressions;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Microsoft.Identity.Client;
using XboxAuthNet.Game.Msal;

namespace Shulkerbox;

public static class ShulkAuthenticator
{
    private static readonly string ClientId = "b7d86c5d-abc0-4002-9eca-a291cce6a053";
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_]{3,16}$");

    public static bool ValidateUsername(string username)
    {
        return UsernameRegex.IsMatch(username);
    }

    public static Task<MSession> AuthenticateOfflineAsync(string username)
    {
        if (!ValidateUsername(username))
            throw new ArgumentException("Invalid username.");
        return Task.FromResult(MSession.CreateOfflineSession(username));
    }

    public static async Task<MSession> AuthenticateInteractiveAsync()
    {
        var app = await MsalClientHelper.BuildApplicationWithCache(ClientId);
        var handler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = handler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(app, msal => msal.Interactive());
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }

    public static async Task<MSession> AuthenticateDeviceCodeAsync(Action<DeviceCodeResult> callback)
    {
        var app = await MsalClientHelper.BuildApplicationWithCache(ClientId);
        var handler = JELoginHandlerBuilder.BuildDefault();
        var authenticator = handler.CreateAuthenticatorWithNewAccount();
        authenticator.AddMsalOAuth(
            app,
            msal => msal.DeviceCode(deviceCode =>
            {
                callback(deviceCode);
                return Task.CompletedTask;
            })
        );
        authenticator.AddXboxAuthForJE(xbox => xbox.Basic());
        authenticator.AddJEAuthenticator();
        return await authenticator.ExecuteForLauncherAsync();
    }
}