namespace Admin.NET.MAUI2C;

public static class SharedExtensions
{
    public static MauiAppBuilder RegisterShared(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();
        return builder;
    }
}
