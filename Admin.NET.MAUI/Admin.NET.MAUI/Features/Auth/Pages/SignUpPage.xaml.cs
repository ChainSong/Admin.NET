namespace Admin.NET.MAUI;

public partial class SignUpPage
{
	public SignUpPage(SignUpPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

