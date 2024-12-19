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
        Task AddUserSubscriptionAsync(UserSubscription subscription);
        Task UpdateUserSubscriptionAsync(UserSubscription subscription);
        Task DeleteUserSubscriptionAsync(int id);
    }
}
