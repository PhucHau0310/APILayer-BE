using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<NewsletterSubscription>> GetAllNewsletterSubscriptionsAsync();
        Task<NewsletterSubscription> GetNewsletterSubscriptionByIdAsync(int id);
        Task AddNewsletterSubscriptionAsync(NewsletterSubscription subscription);
        Task UpdateNewsletterSubscriptionAsync(NewsletterSubscription subscription);
        Task DeleteNewsletterSubscriptionAsync(int id);

        Task<IEnumerable<UserSubscription>> GetAllUserSubscriptionsAsync();
        Task<UserSubscription> GetUserSubscriptionByIdAsync(int id);
        Task AddUserSubscriptionAsync(UserSubsReq subscription);
        Task UpdateUserSubscriptionAsync(UserSubsReq subscription);
        Task DeleteUserSubscriptionAsync(int id);
    }
}
