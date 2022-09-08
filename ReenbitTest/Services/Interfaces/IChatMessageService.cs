using ReenbitTest.Entities;

namespace ReenbitTest.Services.Interfaces
{
    public interface IChatMessageService
    {
        public Task<ChatMessage> CreateChatMessage(string text, Guid groupId, Guid userId);
        public Task<IEnumerable<ChatMessage>> GetChatMessagesForUser(Guid groupId, int page, Guid userId);
        public Task<ChatMessage> GetChatMessageById(Guid messageId);
        public Task DeleteChatMessageForAll(ChatMessage chatMessage);
        public Task UpdateChatMessage(ChatMessage chatMessage);
    }
}
