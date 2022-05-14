using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CraftMine.Models;

public partial class AboutDialogViewModel : ObservableObject
{

    [ObservableProperty] private string _text;

    public AboutDialogViewModel()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CraftMine.Resources.Raw.About.md");
        using var streamReader = new StreamReader(stream);
        Text = streamReader.ReadToEnd();
    }

}