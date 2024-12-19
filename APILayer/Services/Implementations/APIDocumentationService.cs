using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class APIDocumentationService : IAPIDocumentationService
    {
        private readonly DbContext _context;

        public APIDocumentationService(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIDocumentation>> GetAllDocumentationsAsync()
        {
            return await _context.Set<APIDocumentation>().ToListAsync();
        }

        public async Task<APIDocumentation?> GetDocumentationByIdAsync(int id)
        {
            return await _context.Set<APIDocumentation>().FindAsync(id);
        }

        public async Task<APIDocumentation> CreateDocumentationAsync(APIDocumentation documentation)
        {
            await _context.Set<APIDocumentation>().AddAsync(documentation);
            await _context.SaveChangesAsync();
            return documentation;
        }

        public async Task<APIDocumentation?> UpdateDocumentationAsync(int id, APIDocumentation updatedDocumentation)
        {
            var existingDocumentation = await _context.Set<APIDocumentation>().FindAsync(id);
            if (existingDocumentation == null)
            {
                return null;
            }

            _context.Entry(existingDocumentation).CurrentValues.SetValues(updatedDocumentation);
            await _context.SaveChangesAsync();
            return existingDocumentation;
        }

        public async Task<bool> DeleteDocumentationAsync(int id)
        {
            var documentation = await _context.Set<APIDocumentation>().FindAsync(id);
            if (documentation == null)
            {
                return false;
            }

            _context.Set<APIDocumentation>().Remove(documentation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
