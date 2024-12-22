using APILayer.Models.Entities;

namespace APILayer.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatMessage> CreateMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetChatHistoryAsync(int senderId, int recipientId);
        Task<IEnumerable<ChatMessage>> GetChatHistoryByIdAsync(int chatId);
        Task<IEnumerable<ChatMessage>> GetChatHistoryByUserIdAsync(int userId);
        Task<ChatMessage> UpdateNotificationAsync(ChatMessage chatMessage);
        Task<bool> DeleteChatAsync(int id);
    }
}
