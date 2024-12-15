using APILayer.Data;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace APILayer.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, string> _userConnections = new();

        public NotificationHub(
            INotificationService notificationService,
            IUserService userService,
            ApplicationDbContext context)
        {
            _notificationService = notificationService;
            _userService = userService;
            _context = context;
        }

        /// <summary>
        /// Sends a notification to a specific user
        /// </summary>
        public async Task SendNotification(string sender, string recipientId, string message)
        {
            var senderId = _userService.GetUserByUsername(sender).Id;
            if (senderId == null || string.IsNullOrEmpty(recipientId)) return;

            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = int.Parse(recipientId),
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Notify only the specific recipient
            if (_userConnections.TryGetValue(recipientId, out var recipientConnectionId))
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceiveNotification", new
                {
                    id = notification.Id,
                    message = notification.Message,
                    createdAt = notification.CreatedAt,
                    sender = sender,
                    isRead = notification.IsRead
                });
            }
        }

        /// Sends a notification to multiple users
        public async Task SendNotificationToMany(string senderUsername, List<string> recipientUsernames, string message)
        {
            foreach (var recipient in recipientUsernames)
            {
                await SendNotification(senderUsername, recipient, message);
            }
        }

        /// Marks a notification as read
        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return;

            notification.IsRead = true;
            //notification.ReadAt = DateTime.UtcNow;

            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();

            await Clients.User(notification.SenderId.ToString())
                .SendAsync("NotificationRead", notificationId);
        }

        /// Gets all unread notifications for a user
        public async Task<List<Notification>> GetUnreadNotifications(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return new List<Notification>();

            return await _context.Notifications
                .Where(n => n.SenderId == user.Id && n.IsRead == false)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        private string IdentityName
        {
            get { return Context.User.Identity.Name; }
        }

        /// Handles client connection
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

        /// Handles client disconnection
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
