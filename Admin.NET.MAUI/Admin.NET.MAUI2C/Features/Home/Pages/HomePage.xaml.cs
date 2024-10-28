

namespace Admin.NET.MAUI2C;

public partial class HomePage
{
   
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
     

    }
}

