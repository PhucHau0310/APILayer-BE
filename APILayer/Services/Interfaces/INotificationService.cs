using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(string sender, string recipient, string message);
        Task<List<Notification>> CreateNotifications(string sender, List<string> recipients, string message);
        Task<bool> MarkNotificationAsRead(int notificationId);
        Task<List<Notification>> GetAllNotifications();
        Task<List<Notification>> GetUnreadNotifications(string username);
        Task<List<Notification>> GetAllNotifications(string username);
        Task<Notification?> GetNotificationById(int id);
        Task<bool> DeleteNotification(int id);
    }
}
