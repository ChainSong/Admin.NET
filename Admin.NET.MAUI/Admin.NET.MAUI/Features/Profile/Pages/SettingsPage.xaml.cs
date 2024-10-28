namespace Admin.NET.MAUI;

public partial class SettingsPage
{
	public SettingsPage(SettingsPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
