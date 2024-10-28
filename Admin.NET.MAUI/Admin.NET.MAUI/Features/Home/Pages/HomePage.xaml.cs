using LiveChartsCore;

namespace Admin.NET.MAUI;

public partial class HomePage
{
   
	public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
     

    }
}

