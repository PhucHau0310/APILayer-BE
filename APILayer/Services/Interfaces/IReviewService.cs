using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<IEnumerable<Review>> GetReviewsByApiIdAsync(int apiId);
        Task<Review> CreateReviewAsync(ReviewReq request);
        Task<Review?> UpdateReviewAsync(int reviewId, ReviewReq request);
        Task<bool> DeleteReviewAsync(int reviewId);
    }
}
