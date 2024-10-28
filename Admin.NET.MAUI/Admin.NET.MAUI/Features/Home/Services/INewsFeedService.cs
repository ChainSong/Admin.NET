namespace Admin.NET.MAUI;

public interface INewsFeedService
{
    Task<IEnumerable<NewsFeedModel>> GetNewsFeedsAsync(int pageIndex, int pageSize);
}

