using APILayer.Data;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace APILayer.Services.Implementations
{
    public class FAQService : IFAQService
    {
        private readonly ApplicationDbContext _context;

        public FAQService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FAQ>> GetAllFAQsAsync()
        {
            return await _context.FAQs.Include(f => f.User).ToListAsync();
        }

        public async Task<FAQ?> GetFAQByIdAsync(int id)
        {
            return await _context.FAQs.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FAQ>> GetFAQsByUserIdAsync(int userId)
        {
            return await _context.FAQs.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<FAQ> CreateFAQAsync(FAQ faq)
        {
            _context.FAQs.Add(faq);
            await _context.SaveChangesAsync();
            return faq;
        }

        public async Task<FAQ?> UpdateFAQAsync(int id, FAQ faq)
        {
            var existingFAQ = await _context.FAQs.FindAsync(id);
            if (existingFAQ == null) return null;

            existingFAQ.Question = faq.Question;
            existingFAQ.Answer = faq.Answer;
            existingFAQ.Category = faq.Category;
            existingFAQ.UserId = faq.UserId;

            await _context.SaveChangesAsync();
            return existingFAQ;
        }

        public async Task<bool> DeleteFAQAsync(int id)
        {
            var faq = await _context.FAQs.FindAsync(id);
            if (faq == null) return false;

            _context.FAQs.Remove(faq);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
