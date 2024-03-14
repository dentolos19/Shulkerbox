using System.Text.Json.Serialization;
using CmlLib.Core.Auth;

namespace Shulkerbox.Shared.Models;

public class AccountModel
{
    public string Type { get; }
    public MSession Session { get; }
    [JsonIgnore] public Uri HeadImageUrl => new($"https://mc-heads.net/avatar/{Session.Username}/128");

    public AccountModel(MSession session, string type)
    {
        Type = type;
        Session = session;
    }

    public override bool Equals(object? @object)
    {
        return @object is AccountModel model && Session.Username == model.Session.Username;
    }

    public override string ToString()
    {
        return Session.Username;
    }
}