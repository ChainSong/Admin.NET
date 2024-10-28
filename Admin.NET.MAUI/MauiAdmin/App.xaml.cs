using InputKit.Shared.Controls;
using UraniumUI;
using UraniumUI.Resources;

namespace MauiAdmin;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        SelectionView.GlobalSetting.CornerRadius = 0;
        //var dasd= ColorResource.GetColor("Secondary", "SecondaryDark");
        SelectionView.GlobalSetting.Color = ColorResource.GetColor("Secondary", "SecondaryDark");

        MainPage = UraniumServiceProvider.Current.GetRequiredService<AppShell>();
    }
}
