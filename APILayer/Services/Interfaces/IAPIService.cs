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

        Task<IEnumerable<APIDocumentation>> GetAllDocumentationsAsync();
        Task<APIDocumentation?> GetDocumentationByIdAsync(int id);
        Task<APIDocumentation> CreateDocumentationAsync(APIDocumentation documentation);
        Task<APIDocumentation?> UpdateDocumentationAsync(int id, APIDocumentation updatedDocumentation);
        Task<bool> DeleteDocumentationAsync(int id);

        Task<IEnumerable<APIVersion>> GetAllVersionsAsync();
        Task<APIVersion?> GetVersionByIdAsync(int id);
        Task<APIVersion> CreateVersionAsync(APIVersion version);
        Task<APIVersion?> UpdateVersionAsync(int id, APIVersion updatedVersion);
        Task<bool> DeleteVersionAsync(int id);
    }
}
