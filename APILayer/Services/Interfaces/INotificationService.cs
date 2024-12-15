using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int id);
        Task<bool> MarkNotificationAsReadAsync(int id);
    }
}
