namespace Admin.NET.MAUI2C;

public interface IAppSettingsService
{
    Task SetAccessTokenAsync(string value);
    Task SetRefreshTokenAsync(string value);
    Task<string> GetAccessTokenAsync();
    Task<string> GetRefreshTokenAsync();

    string MyPairingId { get; set; }

    bool InARelationship { get; set; }

    bool IsFirstTime { get; set; }

    void Clear();
}

