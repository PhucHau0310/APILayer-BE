using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IFAQService
    {
        Task<IEnumerable<FAQ>> GetAllFAQsAsync();
        Task<FAQ?> GetFAQByIdAsync(int id);
        Task<IEnumerable<FAQ>> GetFAQsByUserIdAsync(int userId);
        Task<FAQ> CreateFAQAsync(FAQReq faqReq);
        Task<FAQ?> UpdateFAQAsync(int id, FAQReq faqReq);
        Task<bool> DeleteFAQAsync(int id);
    }
}
