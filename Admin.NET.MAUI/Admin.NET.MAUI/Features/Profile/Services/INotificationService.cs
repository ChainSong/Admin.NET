namespace Admin.NET.MAUI;

public interface INotificationService
{
    Task<IEnumerable<NotificationModel>> GetNotificationsAsync(int pageIndex, int pageSize);
}

