using APILayer.Data;
using APILayer.Hubs;
using APILayer.Models.Entities;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<ChatMessage> CreateMessageAsync(ChatMessage message)
        {
            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            // Send message to the sender and recipient if connected
            var senderConnection = ChatHub.GetConnectionId(message.Sender.Username);
            var recipientConnection = ChatHub.GetConnectionId(message.Recipient.Username);

            if (senderConnection != null)
            {
                await _hubContext.Clients.Client(senderConnection).SendAsync("ReceiveMessage", new
                {
                    id = message.Id,
                    sender = message.Sender.Username,
                    recipient = message.Recipient.Username,
                    message = message.Message,
                    timestamp = message.Timestamp
                });
            }

            if (recipientConnection != null)
            {
                await _hubContext.Clients.Client(recipientConnection).SendAsync("ReceiveMessage", new
                {
                    id = message.Id,
                    sender = message.Sender.Username,
                    recipient = message.Recipient.Username,
                    message = message.Message,
                    timestamp = message.Timestamp
                });
            }

            return message;
        }

        public async Task<bool> DeleteChatAsync(int id)
        {
            var chatMessage = await _context.ChatMessages.FindAsync(id);
            if (chatMessage == null) return false;

            _context.ChatMessages.Remove(chatMessage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int senderId, int recipientId)
        {
            return await _context.ChatMessages
               .Where(m => (m.SenderId == senderId && m.RecipientId == recipientId) ||
                           (m.SenderId == recipientId && m.RecipientId == senderId))
               .OrderBy(m => m.Timestamp)
               .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryByIdAsync(int chatId)
        {
            return await _context.ChatMessages
                .Where(m => m.Id == chatId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistoryByUserIdAsync(int userId)
        {
            return await _context.ChatMessages
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<ChatMessage> UpdateNotificationAsync(ChatMessage chatMessage)
        {
            _context.ChatMessages.Update(chatMessage);
            await _context.SaveChangesAsync();
            return chatMessage;
        }
    }
}
