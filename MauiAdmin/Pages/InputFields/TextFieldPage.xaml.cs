using MauiAdmin.ViewModels.InputFields;
using UraniumUI;

namespace MauiAdmin.Pages.InputFields;

public partial class TextFieldPage : ContentPage
{
	public TextFieldPage(TextFieldViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}

	void Reset()
	{
		_ = BindingContext;
		BindingContext = UraniumServiceProvider.Current.GetRequiredService<TextFieldViewModel>();
	}

    private void Reset_Clicked(object sender, EventArgs e)
    {
		Reset();
    }
}