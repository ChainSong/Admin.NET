namespace Admin.NET.MAUI;

public static class SharedExtensions
{
    public static MauiAppBuilder RegisterShared(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();
        return builder;
    }
}
