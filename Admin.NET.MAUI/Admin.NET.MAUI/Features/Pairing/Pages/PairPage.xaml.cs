namespace Admin.NET.MAUI;

public partial class PairPage
{
	public PairPage(PairPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

