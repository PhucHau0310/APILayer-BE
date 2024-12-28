using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IAPIService
    {
        Task<IEnumerable<API>> GetAllAPIsAsync();
        Task<API?> GetAPIByIdAsync(int id);
        Task<API> CreateAPIAsync(APIReq apiReq);
        Task<API?> UpdateAPIAsync(int id, APIReq updatedApi);
        Task<bool> DeleteAPIAsync(int id);

        Task<IEnumerable<APIDocumentation>> GetAllDocumentationsAsync();
        Task<APIDocumentation?> GetDocumentationByIdAsync(int id);
        Task<APIDocumentation> CreateDocumentationAsync(APIDocsReq documentation);
        Task<APIDocumentation?> UpdateDocumentationAsync(int id, APIDocsReq updatedDocumentation);
        Task<bool> DeleteDocumentationAsync(int id);

        Task<IEnumerable<APIVersion>> GetAllVersionsAsync();
        Task<APIVersion?> GetVersionByIdAsync(int id);
        Task<APIVersion> CreateVersionAsync(APIVersion version);
        Task<APIVersion?> UpdateVersionAsync(int id, APIVersion updatedVersion);
        Task<bool> DeleteVersionAsync(int id);
    }
}
