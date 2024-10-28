namespace Admin.NET.MAUI2C;

public partial class NotificationsPage
{
	public NotificationsPage(NotificationsPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
