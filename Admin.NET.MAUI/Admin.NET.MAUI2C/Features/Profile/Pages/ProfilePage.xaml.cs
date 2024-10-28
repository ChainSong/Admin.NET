namespace Admin.NET.MAUI2C;

public partial class ProfilePage
{
	public ProfilePage(ProfilePageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}

