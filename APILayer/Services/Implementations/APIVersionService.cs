using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class APIVersionService : IAPIVersionService
    {
        private readonly DbContext _context;

        public APIVersionService(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<APIVersion>> GetAllVersionsAsync()
        {
            return await _context.Set<APIVersion>().ToListAsync();
        }

        public async Task<APIVersion?> GetVersionByIdAsync(int id)
        {
            return await _context.Set<APIVersion>().FindAsync(id);
        }

        public async Task<APIVersion> CreateVersionAsync(APIVersion version)
        {
            await _context.Set<APIVersion>().AddAsync(version);
            await _context.SaveChangesAsync();
            return version;
        }

        public async Task<APIVersion?> UpdateVersionAsync(int id, APIVersion updatedVersion)
        {
            var existingVersion = await _context.Set<APIVersion>().FindAsync(id);
            if (existingVersion == null)
            {
                return null;
            }

            _context.Entry(existingVersion).CurrentValues.SetValues(updatedVersion);
            await _context.SaveChangesAsync();
            return existingVersion;
        }

        public async Task<bool> DeleteVersionAsync(int id)
        {
            var version = await _context.Set<APIVersion>().FindAsync(id);
            if (version == null)
            {
                return false;
            }

            _context.Set<APIVersion>().Remove(version);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
