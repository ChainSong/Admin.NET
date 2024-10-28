namespace Admin.NET.MAUI;

public partial class WalkthroughPage
{
	public WalkthroughPage(WalkthroughPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

