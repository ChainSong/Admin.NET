namespace Admin.NET.MAUI;

public interface ILandingService
{
    Task<IEnumerable<WalkthroughItemModel>> GetWalkthroughItemsAsync();
}