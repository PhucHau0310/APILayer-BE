using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        #region NewsletterSubscription
        [HttpGet("newsletter")]
        public async Task<ActionResult<IEnumerable<NewsletterSubscription>>> GetAllNewsletterSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllNewsletterSubscriptionsAsync();
            return Ok(subscriptions);
        }

        [HttpGet("newsletter/{id}")]
        public async Task<ActionResult<NewsletterSubscription>> GetNewsletterSubscription(int id)
        {
            var subscription = await _subscriptionService.GetNewsletterSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }

        [HttpPost("newsletter")]
        public async Task<ActionResult> AddNewsletterSubscription([FromBody] NewsletterSubscription subscription)
        {
            await _subscriptionService.AddNewsletterSubscriptionAsync(subscription);
            return CreatedAtAction(nameof(GetNewsletterSubscription), new { id = subscription.Id }, subscription);
        }

        [HttpPut("newsletter/{id}")]
        public async Task<ActionResult> UpdateNewsletterSubscription(int id, [FromBody] NewsletterSubscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }
            await _subscriptionService.UpdateNewsletterSubscriptionAsync(subscription);
            return NoContent();
        }

        [HttpDelete("newsletter/{id}")]
        public async Task<ActionResult> DeleteNewsletterSubscription(int id)
        {
            await _subscriptionService.DeleteNewsletterSubscriptionAsync(id);
            return NoContent();
        }
        #endregion

        #region UserSubscription
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<UserSubscription>>> GetAllUserSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllUserSubscriptionsAsync();
            return Ok(subscriptions);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserSubscription>> GetUserSubscription(int id)
        {
            var subscription = await _subscriptionService.GetUserSubscriptionByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }

        [HttpPost("user")]
        public async Task<ActionResult> AddUserSubscription([FromBody] UserSubscription subscription)
        {
            await _subscriptionService.AddUserSubscriptionAsync(subscription);
            return CreatedAtAction(nameof(GetUserSubscription), new { id = subscription.Id }, subscription);
        }

        [HttpPut("user/{id}")]
        public async Task<ActionResult> UpdateUserSubscription(int id, [FromBody] UserSubscription subscription)
        {
            if (id != subscription.Id)
            {
                return BadRequest();
            }
            await _subscriptionService.UpdateUserSubscriptionAsync(subscription);
            return NoContent();
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteUserSubscription(int id)
        {
            await _subscriptionService.DeleteUserSubscriptionAsync(id);
            return NoContent();
        }
        #endregion
    }
}
