using APILayer.Data;
using APILayer.Models.DTOs.Req;
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

        public async Task<FAQ> CreateFAQAsync(FAQReq faqReq)
        {
            var newFAQ = new FAQ
            {
                Question = faqReq.Question,
                Answer = faqReq.Answer,
                Category = faqReq.Category,
                UserId = faqReq.UserId
            };

            _context.FAQs.Add(newFAQ);
            await _context.SaveChangesAsync();
            return newFAQ;
        }

        public async Task<FAQ?> UpdateFAQAsync(int id, FAQReq faqReq)
        {
            var existingFAQ = await _context.FAQs.FindAsync(id);
            if (existingFAQ == null) return null;

            existingFAQ.Question = faqReq.Question;
            existingFAQ.Answer = faqReq.Answer;
            existingFAQ.Category = faqReq.Category;
            existingFAQ.UserId = faqReq.UserId;

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
