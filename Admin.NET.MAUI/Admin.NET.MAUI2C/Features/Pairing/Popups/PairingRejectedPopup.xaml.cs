namespace Admin.NET.MAUI2C;

public partial class PairingRejectedPopup
{
	public PairingRejectedPopup(PairingRejectedPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
