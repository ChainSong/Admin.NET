﻿namespace Admin.NET.MAUI
{
    public partial class PairingAcceptedPopupViewModel(IAppNavigator appNavigator) : BaseViewModel(appNavigator)
    {

        [RelayCommand]
        private Task Done(object obj) => AppNavigator.NavigateAsync(AppRoutes.Home);
    }
}