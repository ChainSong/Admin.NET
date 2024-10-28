namespace Admin.NET.MAUI2C;

public interface INotificationService
{
    Task<IEnumerable<NotificationModel>> GetNotificationsAsync(int pageIndex, int pageSize);
}

