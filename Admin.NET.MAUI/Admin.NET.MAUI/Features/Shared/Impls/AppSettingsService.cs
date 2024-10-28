namespace Admin.NET.MAUI;

public class AppSettingsService(
        IPreferences preferences,
        ISecureStorage secureStorage
        ) : IAppSettingsService
{
    private readonly IPreferences preferences = preferences;
    private readonly ISecureStorage secureStorage = secureStorage;

    public string MyPairingId
    {
        get => preferences.Get<string>(nameof(MyPairingId), "123789", nameof(Admin.NET.MAUI));
        set => preferences.Set<string>(nameof(MyPairingId), value, nameof(Admin.NET.MAUI));
    }

    public bool IsFirstTime
    {
        get => preferences.Get<bool>(nameof(IsFirstTime), true, nameof(Admin.NET.MAUI));
        set => preferences.Set<bool>(nameof(IsFirstTime), value, nameof(Admin.NET.MAUI));
    }

    public async Task SetAccessTokenAsync(string value)
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // TODO Uncomment for real app with Apple Developer Account
            // await secureStorage.SetAsync(nameof(SetAccessTokenAsync), value);
            preferences.Set<string>(nameof(SetAccessTokenAsync), value, nameof(Admin.NET.MAUI));
            return;
        }

        await secureStorage.SetAsync(nameof(SetAccessTokenAsync), value);
    }

    public async Task SetRefreshTokenAsync(string value)
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // TODO Uncomment for real app with Apple Developer Account
            // await secureStorage.SetAsync(nameof(SetAccessTokenAsync), value);
            preferences.Set<string>(nameof(SetRefreshTokenAsync), value, nameof(Admin.NET.MAUI));
            return;
        }

        await secureStorage.SetAsync(nameof(SetRefreshTokenAsync), value);
    }
    public async Task<string> GetAccessTokenAsync()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // TODO Uncomment for real app with Apple Developer Account
            // return await secureStorage.GetAsync(nameof(SetAccessTokenAsync));

            return preferences.Get<string>(nameof(SetAccessTokenAsync), null, nameof(Admin.NET.MAUI));
        }

        return await secureStorage.GetAsync(nameof(SetAccessTokenAsync));
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // TODO Uncomment for real app with Apple Developer Account
            // return await secureStorage.GetAsync(nameof(SetAccessTokenAsync));

            return preferences.Get<string>(nameof(SetRefreshTokenAsync), null, nameof(Admin.NET.MAUI));
        }

        return await secureStorage.GetAsync(nameof(SetRefreshTokenAsync));
    }

    public bool InARelationship
    {
        get => preferences.Get<bool>(nameof(InARelationship), false, nameof(Admin.NET.MAUI));
        set => preferences.Set<bool>(nameof(InARelationship), value, nameof(Admin.NET.MAUI));
    }

    public void Clear()
    {
        preferences.Clear();
        secureStorage.RemoveAll();
        IsFirstTime = false;
    }
}

