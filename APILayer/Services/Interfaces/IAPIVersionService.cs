using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IAPIVersionService
    {
        Task<IEnumerable<APIVersion>> GetAllVersionsAsync();
        Task<APIVersion?> GetVersionByIdAsync(int id);
        Task<APIVersion> CreateVersionAsync(APIVersion version);
        Task<APIVersion?> UpdateVersionAsync(int id, APIVersion updatedVersion);
        Task<bool> DeleteVersionAsync(int id);
    }
}
