using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CraftMine.Models;

public partial class AboutDialogModel : ObservableObject
{

    [ObservableProperty] private string _text;

    public AboutDialogModel()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CraftMine.Resources.Raw.About.md");
        using var streamReader = new StreamReader(stream);
        Text = streamReader.ReadToEnd();
    }

}