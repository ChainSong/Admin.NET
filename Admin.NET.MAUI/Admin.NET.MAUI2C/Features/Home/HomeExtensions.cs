namespace Admin.NET.MAUI2C;

public static class HomeExtensions
{
    public static MauiAppBuilder RegisterHome(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<INewsFeedService, NewsFeedService>();
        return builder;
    }
}
