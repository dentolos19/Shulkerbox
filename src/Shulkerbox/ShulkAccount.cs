using CmlLib.Core.Auth;

namespace Shulkerbox;

public class ShulkAccount
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Username { get; init; }
    public ShulkAccountType Type { get; init; }

    public string GetHeadUrl(int size = 128)
    {
        return $"https://mc-heads.net/avatar/{Username}/{size}";
    }

    public Task<MSession> CreateSessionAsync()
    {
        return Type switch
        {
            ShulkAccountType.Official => throw new NotImplementedException(),
            ShulkAccountType.Offline => ShulkAuthenticator.AuthenticateOfflineAsync(Username),
            _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, default)
        };
    }
}