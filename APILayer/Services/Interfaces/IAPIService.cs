using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IAPIService
    {
        Task<IEnumerable<API>> GetAllAPIsAsync();
        Task<API?> GetAPIByIdAsync(int id);
        Task<API> CreateAPIAsync(API api);
        Task<API?> UpdateAPIAsync(int id, API updatedApi);
        Task<bool> DeleteAPIAsync(int id);
    }
}
