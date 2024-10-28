namespace Admin.NET.MAUI2C;

public interface INewsFeedService
{
    Task<IEnumerable<NewsFeedModel>> GetNewsFeedsAsync(int pageIndex, int pageSize);
}

