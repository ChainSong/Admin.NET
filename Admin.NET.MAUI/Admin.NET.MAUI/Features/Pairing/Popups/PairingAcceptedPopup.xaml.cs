namespace Admin.NET.MAUI;

public partial class PairingAcceptedPopup
{
	public PairingAcceptedPopup(PairingAcceptedPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
