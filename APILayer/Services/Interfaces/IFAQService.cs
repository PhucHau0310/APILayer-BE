using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IFAQService
    {
        Task<IEnumerable<FAQ>> GetAllFAQsAsync();
        Task<FAQ?> GetFAQByIdAsync(int id);
        Task<IEnumerable<FAQ>> GetFAQsByUserIdAsync(int userId);
        Task<FAQ> CreateFAQAsync(FAQ faq);
        Task<FAQ?> UpdateFAQAsync(int id, FAQ faq);
        Task<bool> DeleteFAQAsync(int id);
    }
}
