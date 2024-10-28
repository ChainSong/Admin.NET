namespace Admin.NET.MAUI;
public static class PairingExtensions
{
    public static MauiAppBuilder RegisterPairing(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IPairingService, PairingService>();
        return builder;
    }
}
