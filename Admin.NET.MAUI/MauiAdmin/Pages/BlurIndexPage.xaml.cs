using MauiAdmin.Pages.Blurs;

namespace MauiAdmin.Pages;

public partial class BlurIndexPage : ContentPage
{
	public BlurIndexPage()
	{
		InitializeComponent();
	}

    private void GoToPreviewPage(object sender, EventArgs e)
    {
		this.Navigation.PushAsync(new BlursPreviewPage());
    }
}