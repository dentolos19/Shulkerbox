using System;
using System.Threading.Tasks;
using CraftMine.Services;

namespace CraftMine.Models;

public class AccountItemModel
{

    public Uri ImageUrl { get; set; }
    public string Username { get; set; }

    public AccountItemModel(string username)
    {
        Username = username;
        Task.Run(async () => ImageUrl = await GameService.Instance.GetHeadImageUrl(Username));
    }

}