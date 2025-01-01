using APILayer.Models.DTOs.Req;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetReviewsByUser(int userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        [HttpGet("api/{apiId}")]
        public async Task<IActionResult> GetReviewsByApi(int apiId)
        {
            var reviews = await _reviewService.GetReviewsByApiIdAsync(apiId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewReq request)
        {
            var review = await _reviewService.CreateReviewAsync(request);
            return CreatedAtAction(nameof(GetReviewById), new { id = review.ReviewId }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, ReviewReq request)
        {
            var review = await _reviewService.UpdateReviewAsync(id, request);
            if (review == null) return NotFound();
            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
