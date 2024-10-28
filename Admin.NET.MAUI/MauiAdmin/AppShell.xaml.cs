using System.Collections.ObjectModel;

namespace MauiAdmin;

public partial class AppShell : Shell
{
    public List<AppShellItem> MenuItems { get; set; } = new List<AppShellItem>
    {
        new AppShellItem { Title = "Home", Icon = "home.png", Page = "HomePage" },
        new AppShellItem { Title = "Customers", Icon = "customers.png", Page = "CustomersPage" },
        new AppShellItem { Title = "Orders", Icon = "orders.png", Page = "OrdersPage" },
        new AppShellItem { Title = "Settings", Icon = "settings.png", Page = "SettingsPage" }
    };
    public AppShell()
    {
        InitializeComponent();
        themeSwitch.IsToggled = App.Current.RequestedTheme == AppTheme.Dark;
        App.Current.RequestedThemeChanged += (s, e) =>
        {
            themeSwitch.IsToggled = App.Current.RequestedTheme == AppTheme.Dark;
        };
    }

    private void ThemeToggled(object sender, ToggledEventArgs e)
    {
        App.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
    }
}


public class AppShellItem {
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Page { get; set; }
}