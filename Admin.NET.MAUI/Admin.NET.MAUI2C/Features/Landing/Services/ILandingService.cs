namespace Admin.NET.MAUI2C;

public interface ILandingService
{
    Task<IEnumerable<WalkthroughItemModel>> GetWalkthroughItemsAsync();
}