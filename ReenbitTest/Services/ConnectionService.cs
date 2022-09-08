using Microsoft.EntityFrameworkCore;
using ReenbitTest.Context;
using ReenbitTest.Entities;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Services
{
    public class ConnectionService : IConnectionService
    {
        private ChatContext _dbContext;

        public ConnectionService(ChatContext context)
        {
            _dbContext = context;
        }

        public async Task AddConnection(string connectionId, Guid userId)
        {
            var connection = new Connection
            {
                ConnectionId = connectionId,
                UserId = userId
            };

            await _dbContext.Connections.AddAsync(connection);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<Connection>> GetConnectionsByUserId(Guid userId)
        {
            return await _dbContext.Connections.Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
