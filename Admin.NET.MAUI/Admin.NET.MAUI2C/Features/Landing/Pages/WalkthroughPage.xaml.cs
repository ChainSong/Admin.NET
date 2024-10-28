namespace Admin.NET.MAUI2C;

public partial class WalkthroughPage
{
	public WalkthroughPage(WalkthroughPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

