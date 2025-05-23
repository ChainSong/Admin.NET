﻿namespace Admin.NET.MAUI;

public static class LandingExtensions
{
    public static MauiAppBuilder RegisterLanding(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ILandingService, LandingService>();
        return builder;
    }
}
