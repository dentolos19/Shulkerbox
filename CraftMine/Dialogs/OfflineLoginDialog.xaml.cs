using Microsoft.UI.Xaml.Controls;

namespace CraftMine.Dialogs;

public sealed partial class OfflineLoginDialog
{

    public string? Username { get; private set; }

    public OfflineLoginDialog()
    {
        InitializeComponent();
    }

    private void OnPrimaryButtonClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        Username = UsernameBox.Text;
    }

}