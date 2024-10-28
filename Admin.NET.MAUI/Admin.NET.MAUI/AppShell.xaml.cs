namespace Admin.NET.MAUI
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
