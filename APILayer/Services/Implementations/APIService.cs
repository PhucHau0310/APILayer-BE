using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class APIService : IAPIService
    {
        private readonly DbContext _context;

        public APIService(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<API>> GetAllAPIsAsync()
        {
            return await _context.Set<API>().Include(a => a.Versions)
                                          .Include(a => a.Documentations)
                                          .Include(a => a.Reviews)
                                          .ToListAsync();
        }

        public async Task<API?> GetAPIByIdAsync(int id)
        {
            return await _context.Set<API>().Include(a => a.Versions)
                                          .Include(a => a.Documentations)
                                          .Include(a => a.Reviews)
                                          .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<API> CreateAPIAsync(API api)
        {
            await _context.Set<API>().AddAsync(api);
            await _context.SaveChangesAsync();
            return api;
        }

        public async Task<API?> UpdateAPIAsync(int id, API updatedApi)
        {
            var existingApi = await _context.Set<API>().FindAsync(id);
            if (existingApi == null)
            {
                return null;
            }

            _context.Entry(existingApi).CurrentValues.SetValues(updatedApi);
            await _context.SaveChangesAsync();
            return existingApi;
        }

        public async Task<bool> DeleteAPIAsync(int id)
        {
            var api = await _context.Set<API>().FindAsync(id);
            if (api == null)
            {
                return false;
            }

            _context.Set<API>().Remove(api);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
