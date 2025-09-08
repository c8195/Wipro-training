namespace DoConnect.API.Services
{
    public interface INotificationService
    {
        Task NotifyAdminsAsync(string title, string message);
        Task NotifyUserAsync(int userId, string title, string message);
        Task<List<object>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 10);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}
