using APILayer.Data;
using APILayer.Hubs;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;

namespace APILayer.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            ApplicationDbContext context,
            IUserService userService,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _userService = userService;
            _hubContext = hubContext;
        }

        public async Task<Notification> CreateNotification(string sender, string recipient, string message)
        {
            //var senderId = _userService.GetUserByUsername(sender)?.Id;
            //var recipientId = _userService.GetUserByUsername(recipient)?.Id;
            var senderUser = await _userService.GetUserByUsernameAsync(sender);
            var recipientUser = await _userService.GetUserByUsernameAsync(recipient);

            if (senderUser == null || recipientUser == null)
                throw new ArgumentException("Invalid sender or recipient");

            var notification = new Notification
            {
                SenderId = senderUser.Id,
                ReceiverId = recipientUser.Id,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Send real-time notification
            var connectionId = NotificationHub.GetConnectionId(recipient);
            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", new
                {
                    id = notification.Id,
                    message = notification.Message,
                    createdAt = notification.CreatedAt,
                    sender = sender,
                    isRead = notification.IsRead
                });
            }

            return notification;
        }

        public async Task<List<Notification>> CreateNotifications(string sender, List<string> recipients, string message)
        {
            var notifications = new List<Notification>();
            foreach (var recipient in recipients)
            {
                var notification = await CreateNotification(sender, recipient, message);
                notifications.Add(notification);
            }
            return notifications;
        }

        public async Task<bool> MarkNotificationAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();

            // Notify sender that notification was read
            await _hubContext.Clients.User(notification.SenderId.ToString())
                .SendAsync("NotificationRead", notificationId);

            return true;
        }

        public async Task<List<Notification>> GetUnreadNotifications(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return new List<Notification>();

            return await _context.Notifications
                .Where(n => n.ReceiverId == user.Id && (n.IsRead == null || n.IsRead == false))
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetAllNotifications(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return new List<Notification>();

            return await _context.Notifications
                .Where(n => n.ReceiverId == user.Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetNotificationById(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<bool> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return false;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
