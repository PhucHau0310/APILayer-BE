using APILayer.Data;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;

namespace APILayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, string> _userConnections = new();
        public ChatHub(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        public async Task SendMessage(string sender, string recipient, string message)
        {
            var senderId = _userService.GetUserByUsername(sender)?.Id;
            var recipientId = _userService.GetUserByUsername(recipient)?.Id;
            if (senderId == null || recipientId == null) return;

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // Send message to only involved users
            await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", sender, message);
            await Clients.User(recipientId.ToString()).SendAsync("ReceiveMessage", sender, message);
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;
            Console.WriteLine("Userid is " + userId);
            if (userId != null)
            {
                _userConnections[userId] = Context.ConnectionId;
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.Identity?.Name;
            if (userId != null)
            {
                _userConnections.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public static string? GetConnectionId(string username)
        {
            return _userConnections.TryGetValue(username, out var connectionId) ? connectionId : null;
        }
    }
}
