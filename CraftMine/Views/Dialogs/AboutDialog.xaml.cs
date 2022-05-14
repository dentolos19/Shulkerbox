using System;
using Windows.System;
using CommunityToolkit.WinUI.UI.Controls;

namespace CraftMine.Views.Dialogs;

public sealed partial class AboutDialog
{

    public AboutDialog()
    {
        InitializeComponent();
    }

    private async void OnLinkClicked(object sender, LinkClickedEventArgs args)
    {
        await Launcher.LaunchUriAsync(new Uri(args.Link));
    }

}