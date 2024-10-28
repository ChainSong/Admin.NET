namespace Admin.NET.MAUI;

public partial class PartnerFoundPage
{
	public PartnerFoundPage(PartnerFoundPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

