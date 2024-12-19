using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IAPIDocumentationService
    {
        Task<IEnumerable<APIDocumentation>> GetAllDocumentationsAsync();
        Task<APIDocumentation?> GetDocumentationByIdAsync(int id);
        Task<APIDocumentation> CreateDocumentationAsync(APIDocumentation documentation);
        Task<APIDocumentation?> UpdateDocumentationAsync(int id, APIDocumentation updatedDocumentation);
        Task<bool> DeleteDocumentationAsync(int id);
    }
}
