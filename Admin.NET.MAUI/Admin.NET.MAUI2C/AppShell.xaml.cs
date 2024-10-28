namespace Admin.NET.MAUI2C
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("notifications", typeof(NotificationsPage));
        }
    }
}
