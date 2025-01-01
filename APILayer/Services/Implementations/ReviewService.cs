using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Api)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Api)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Api)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByApiIdAsync(int apiId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ApiId == apiId)
                .ToListAsync();
        }

        public async Task<Review> CreateReviewAsync(ReviewReq request)
        {
            var review = new Review
            {
                UserId = request.UserId,
                ApiId = request.ApiId,
                Rating = request.Rating,
                Comment = request.Comment,
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> UpdateReviewAsync(int reviewId, ReviewReq request)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return null;

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
