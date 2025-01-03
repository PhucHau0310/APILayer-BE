using APILayer.Models.DTOs.Req;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetAllNotifications();
            return Ok(notifications);
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationReq request)
        {
            try
            {
                var notification = await _notificationService.CreateNotification(
                    request.Sender,
                    request.Recipient,
                    request.Message
                );
                return Ok(notification);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("send-many")]
        public async Task<IActionResult> SendNotificationToMany([FromBody] NotificationBulkReq request)
        {
            try
            {
                var notifications = await _notificationService.CreateNotifications(
                    request.Sender,
                    request.Recipients,
                    request.Message
                );
                return Ok(notifications);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _notificationService.MarkNotificationAsRead(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpGet("unread/{username}")]
        public async Task<IActionResult> GetUnreadNotifications(string username)
        {
            var notifications = await _notificationService.GetUnreadNotifications(username);
            return Ok(notifications);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetAllNotifications(string username)
        {
            var notifications = await _notificationService.GetAllNotifications(username);
            return Ok(notifications);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationById(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var success = await _notificationService.DeleteNotification(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
