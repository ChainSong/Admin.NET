namespace Admin.NET.MAUI2C;

public partial class LandingPage
{
	public LandingPage(LandingPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

