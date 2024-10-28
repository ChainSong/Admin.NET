namespace Admin.NET.MAUI2C;

public partial class ForgotPasswordPage
{
	public ForgotPasswordPage(ForgotPasswordPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

