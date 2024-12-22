using APILayer.Models.DTOs.Req;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatController(IUserService userService, IChatService chatService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto chatMessage)
        {
            Console.WriteLine("Nguoi gui: " + chatMessage.Sender + "Nguoi nhan: " + chatMessage.Recipient);
            if (chatMessage == null) return BadRequest("Message cannot be null");

            var sender = await _userService.GetUserByUsername(chatMessage.Sender);
            var recipient = await _userService.GetUserByUsername(chatMessage.Recipient);

            if (sender == null || recipient == null) return BadRequest("Invalid sender or recipient");

            var chatMsg = new ChatMessage
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Message = chatMessage.Message,
                Timestamp = DateTime.UtcNow
            };

            await _chatService.CreateMessageAsync(chatMsg);

            return Ok(new
            {
                id = chatMsg.Id,
                sender = new { username = sender.Username },
                recipient = new { username = recipient.Username },
                message = chatMsg.Message,
                timestamp = chatMsg.Timestamp
            });
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory(string user1, string user2)
        {
            var user1Entity = await _userService.GetUserByUsername(user1);
            var user2Entity = await _userService.GetUserByUsername(user2);

            if (user1Entity == null || user2Entity == null)
                return BadRequest("Invalid users");

            var messages = await _chatService.GetChatHistoryAsync(user1Entity.Id, user2Entity.Id);
            return Ok(messages);
        }

        [HttpGet("get-conversations/{username}")]
        public async Task<IActionResult> GetConversations(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound("User not found");

            var conversations = await _chatService.GetChatHistoryByUserIdAsync(user.Id);
            return Ok(conversations);
        }
    }
}
