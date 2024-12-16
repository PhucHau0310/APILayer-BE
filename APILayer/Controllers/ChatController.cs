using APILayer.Data;
using APILayer.Hubs;
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public ChatController(IHubContext<ChatHub> chatHub, IUserService userService, ApplicationDbContext dbContext)
        {
            _chatHub = chatHub;
            _userService = userService;
            _context = dbContext;
        }

        [HttpPost("sendAll")]
        public async Task<IActionResult> SendMessage(string user, string message)
        {
            await _chatHub.Clients.All.SendAsync("ReceiveMessage", user, message);
            return Ok(new Response<string>
            {
                Success = true,
                Message = "Message sent",
                Data = null
            });
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

            _context.ChatMessages.Add(chatMsg);
            await _context.SaveChangesAsync();

            // Retrieve connection IDs for both sender and recipient
            var senderConnection = ChatHub.GetConnectionId(chatMessage.Sender);
            var recipientConnection = ChatHub.GetConnectionId(chatMessage.Recipient);

            // Send message to the sender if they are connected
            if (senderConnection != null)
            {
                await _chatHub.Clients.Client(senderConnection).SendAsync("ReceiveMessage", new
                {
                    id = chatMsg.Id,
                    sender = chatMessage.Sender,
                    recipient = chatMessage.Recipient,
                    message = chatMessage.Message,
                    timestamp = chatMsg.Timestamp
                });
            }

            // Send message to the recipient if they are connected
            if (recipientConnection != null)
            {
                await _chatHub.Clients.Client(recipientConnection).SendAsync("ReceiveMessage", new
                {
                    id = chatMsg.Id,
                    sender = chatMessage.Sender,
                    recipient = chatMessage.Recipient,
                    message = chatMessage.Message,
                    timestamp = chatMsg.Timestamp
                });
            }

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
            var user1Entity = await _context.Users.FirstOrDefaultAsync(u => u.Username == user1);
            var user2Entity = await _context.Users.FirstOrDefaultAsync(u => u.Username == user2);
            if (user1Entity == null || user2Entity == null)
                return BadRequest("Invalid users");

            // Fetch messages where either user is sender and the other is recipient
            var user1Id = user1Entity.Id;
            var user2Id = user2Entity.Id;

            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == user1Id && m.RecipientId == user2Id) ||
                            (m.SenderId == user2Id && m.RecipientId == user1Id))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpGet("get-conversations/{username}")]
        public async Task<IActionResult> GetConversations(string username)
        {
            var user = await _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound("User not found");

            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .Where(m => m.Sender.Username == username || m.Recipient.Username == username)
                .ToListAsync();

            var conversations = messages
                .GroupBy(m => m.Sender.Username == username ? m.Recipient : m.Sender)
                .Select(group => new
                {
                    userId = group.Key.Id,
                    name = group.Key.Username,
                    lastMessage = group.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Message,
                    lastMessageTime = group.OrderByDescending(m => m.Timestamp).FirstOrDefault()?.Timestamp
                })
                .ToList();

            return Ok(conversations);
        }
    }
}
