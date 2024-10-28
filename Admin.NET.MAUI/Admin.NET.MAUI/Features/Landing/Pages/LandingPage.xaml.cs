namespace Admin.NET.MAUI;

public partial class LandingPage
{
	public LandingPage(LandingPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

