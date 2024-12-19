using APILayer.Data;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class FeaturedAPIService : IFeaturedAPIService
    {
        private readonly ApplicationDbContext _context;

        public FeaturedAPIService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FeaturedAPI>> GetAllFeaturedAPIsAsync()
        {
            return await _context.FeaturedAPIs
                .Include(f => f.Api)
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<FeaturedAPI?> GetFeaturedAPIByIdAsync(int id)
        {
            return await _context.FeaturedAPIs
                .Include(f => f.Api)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FeaturedAPI>> GetFeaturedAPIsByUserIdAsync(int userId)
        {
            return await _context.FeaturedAPIs
                .Include(f => f.Api)
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FeaturedAPI>> GetCurrentFeaturedAPIsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.FeaturedAPIs
                .Include(f => f.Api)
                .Include(f => f.User)
                .Where(f => f.FeaturedFrom <= currentDate && f.FeaturedTo >= currentDate)
                .ToListAsync();
        }

        public async Task<FeaturedAPI> CreateFeaturedAPIAsync(FeaturedAPI featuredAPI)
        {
            _context.FeaturedAPIs.Add(featuredAPI);
            await _context.SaveChangesAsync();
            return featuredAPI;
        }

        public async Task<FeaturedAPI?> UpdateFeaturedAPIAsync(int id, FeaturedAPI featuredAPI)
        {
            var existingFeaturedAPI = await _context.FeaturedAPIs.FindAsync(id);
            if (existingFeaturedAPI == null)
                return null;

            existingFeaturedAPI.ApiId = featuredAPI.ApiId;
            existingFeaturedAPI.UserId = featuredAPI.UserId;
            existingFeaturedAPI.ReasonForFeature = featuredAPI.ReasonForFeature;
            existingFeaturedAPI.FeaturedFrom = featuredAPI.FeaturedFrom;
            existingFeaturedAPI.FeaturedTo = featuredAPI.FeaturedTo;

            await _context.SaveChangesAsync();
            return existingFeaturedAPI;
        }

        public async Task<bool> DeleteFeaturedAPIAsync(int id)
        {
            var featuredAPI = await _context.FeaturedAPIs.FindAsync(id);
            if (featuredAPI == null)
                return false;

            _context.FeaturedAPIs.Remove(featuredAPI);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
