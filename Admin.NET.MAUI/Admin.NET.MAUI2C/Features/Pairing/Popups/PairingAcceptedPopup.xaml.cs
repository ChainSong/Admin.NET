namespace Admin.NET.MAUI2C;

public partial class PairingAcceptedPopup
{
	public PairingAcceptedPopup(PairingAcceptedPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
