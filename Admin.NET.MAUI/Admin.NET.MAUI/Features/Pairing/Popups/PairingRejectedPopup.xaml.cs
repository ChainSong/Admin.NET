namespace Admin.NET.MAUI;

public partial class PairingRejectedPopup
{
	public PairingRejectedPopup(PairingRejectedPopupViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}
