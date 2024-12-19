using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IFeaturedAPIService
    {
        Task<IEnumerable<FeaturedAPI>> GetAllFeaturedAPIsAsync();
        Task<FeaturedAPI?> GetFeaturedAPIByIdAsync(int id);
        Task<IEnumerable<FeaturedAPI>> GetFeaturedAPIsByUserIdAsync(int userId);
        Task<IEnumerable<FeaturedAPI>> GetCurrentFeaturedAPIsAsync();
        Task<FeaturedAPI> CreateFeaturedAPIAsync(FeaturedAPI featuredAPI);
        Task<FeaturedAPI?> UpdateFeaturedAPIAsync(int id, FeaturedAPI featuredAPI);
        Task<bool> DeleteFeaturedAPIAsync(int id);
    }
}
