using MauiAdmin.ViewModels;
using UraniumUI;
using UraniumUI.Dialogs.Mopups;
using UraniumUI.Material.Controls;

namespace MauiAdmin.Pages;

public partial class ChipsPage : ContentPage
{
	public ChipsPage()
	{
		BindingContext = UraniumServiceProvider.Current.GetRequiredService<ChipsViewModel>();
		InitializeComponent();
	}
}