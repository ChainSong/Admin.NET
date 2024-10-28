namespace Admin.NET.MAUI;

public partial class SettingsAndHelpPage
{
	public SettingsAndHelpPage(SettingsAndHelpPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
