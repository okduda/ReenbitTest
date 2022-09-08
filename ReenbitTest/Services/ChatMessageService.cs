using Microsoft.EntityFrameworkCore;
using ReenbitTest.Context;
using ReenbitTest.Entities;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private const int PAGE_SIZE = 20;
        private ChatContext _dbContext;

        public ChatMessageService(ChatContext context)
        {
            _dbContext = context;
        }

        public async Task<ChatMessage> CreateChatMessage(string text, Guid groupId, Guid userId)
        {
            var chatMessage = new ChatMessage
            {
                GroupId = groupId,
                UserId = userId,
                Text = text,
                IsDeletedForUser = false
            };

            await _dbContext.ChatMessages.AddAsync(chatMessage);

            await _dbContext.SaveChangesAsync();

            return chatMessage;
        }

        public async Task<IEnumerable<ChatMessage>> GetChatMessagesForUser(Guid groupId, int page, Guid userId)
        {
            return await _dbContext.ChatMessages
                .Include(cm => cm.User)
                .Where(cm => cm.GroupId == groupId &&
                    !(cm.UserId == userId && cm.IsDeletedForUser))
                .OrderByDescending(cm => cm.CreationTime)
                .Skip(page * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();
        }

        public async Task<ChatMessage> GetChatMessageById(Guid messageId)
        {
            return await _dbContext.ChatMessages.FirstOrDefaultAsync(cm => cm.Id == messageId);
        }

        public async Task DeleteChatMessageForAll(ChatMessage chatMessage)
        {
            _dbContext.ChatMessages.Remove(chatMessage);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateChatMessage(ChatMessage chatMessage)
        {
            _dbContext.ChatMessages.Update(chatMessage);

            await _dbContext.SaveChangesAsync();
        }
    }
}