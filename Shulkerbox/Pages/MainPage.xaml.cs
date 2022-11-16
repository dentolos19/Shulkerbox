using System.Windows;
using Shulkerbox.Models;

namespace Shulkerbox.Pages;

public partial class MainPage
{

    private MainPageModel Model => (MainPageModel)DataContext;

    public MainPage()
    {
        InitializeComponent();
        DataContext = App.GetModel<MainPageModel>();
    }

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
        Model.RefreshCommand.Execute(null);
    }

}