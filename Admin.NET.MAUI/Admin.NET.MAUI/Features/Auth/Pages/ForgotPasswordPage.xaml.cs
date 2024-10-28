namespace Admin.NET.MAUI;

public partial class ForgotPasswordPage
{
	public ForgotPasswordPage(ForgotPasswordPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

