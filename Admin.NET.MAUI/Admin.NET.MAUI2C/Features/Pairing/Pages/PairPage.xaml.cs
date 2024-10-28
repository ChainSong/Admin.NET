namespace Admin.NET.MAUI2C;

public partial class PairPage
{
	public PairPage(PairPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

