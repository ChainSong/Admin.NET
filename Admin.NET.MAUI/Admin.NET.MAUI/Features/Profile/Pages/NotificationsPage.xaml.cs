namespace Admin.NET.MAUI;

public partial class NotificationsPage
{
	public NotificationsPage(NotificationsPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
