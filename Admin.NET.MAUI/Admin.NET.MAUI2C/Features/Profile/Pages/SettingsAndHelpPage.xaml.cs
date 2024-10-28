namespace Admin.NET.MAUI2C;

public partial class SettingsAndHelpPage
{
	public SettingsAndHelpPage(SettingsAndHelpPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
