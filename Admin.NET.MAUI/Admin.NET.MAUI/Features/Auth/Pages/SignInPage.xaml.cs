﻿using Admin.NET.Entity;
using System.Net;

namespace Admin.NET.MAUI;

public partial class SignInPage
{
	public SignInPage(SignInPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        //AddCookies();
    }


	
}
