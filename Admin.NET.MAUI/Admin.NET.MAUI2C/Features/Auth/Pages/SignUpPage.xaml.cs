namespace Admin.NET.MAUI2C;
public partial class SignUpPage
{
	public SignUpPage(SignUpPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

