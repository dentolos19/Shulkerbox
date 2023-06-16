namespace Shulkerbox.Models;

public class AccountModel
{
    public string Username { get; init; }
    public string Type { get; init; }
    public Uri HeadImageUrl => new($"https://mc-heads.net/avatar/{Username}/128");

    public override bool Equals(object? @object)
    {
        return @object is AccountModel model && Username == model.Username;
    }

    public override string ToString()
    {
        return Username;
    }
}