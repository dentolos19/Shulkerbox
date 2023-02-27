using System;
using Shulkerbox.Services;

namespace Shulkerbox.Models;

public class AccountItemModel
{
    public Uri ImageUrl { get; set; }
    public string Username { get; set; }

    public AccountItemModel(string username)
    {
        Username = username;
        ImageUrl = GameService.Instance.GetHeadImageUrl(Username);
    }
}