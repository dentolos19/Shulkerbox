using System.Text.Json.Serialization;
using CmlLib.Core.Auth;

namespace Shulkerbox.Shared.Objects;

public class MinecraftAccount(MSession session, MinecraftAccountType type)
{
    public MinecraftAccountType Type { get; } = type;
    public MSession Session { get; } = session;
    [JsonIgnore] public Uri HeadImageUrl => new($"https://mc-heads.net/avatar/{Session.Username}/128");

    public override bool Equals(object? obj)
    {
        return
            obj is MinecraftAccount model &&
            Session.Username == model.Session.Username;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, Session.Username);
    }

    public override string? ToString()
    {
        return Session.Username;
    }
}