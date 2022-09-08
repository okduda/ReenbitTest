using Microsoft.EntityFrameworkCore;
using ReenbitTest.Context;
using ReenbitTest.Entities;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Services
{
    public class UserService : IUserService
    {
        private ChatContext _dbContext;

        public UserService(ChatContext context)
        {
            _dbContext = context;
        }

        public async Task<User> GetOrCreateUser(string userName)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                user = new User();
                user.UserName = userName;
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> GetUserByConnectionId(string connectionId)
        {
            return await _dbContext.Users
                .Include(u => u.Connections)
                .FirstOrDefaultAsync(u => u.Connections.Any(c => c.ConnectionId == connectionId));
        }

        public async Task<IEnumerable<User>> GetUsersByUserNames(IEnumerable<string> userNames)
        {
            return await _dbContext.Users.Where(u => userNames.Contains(u.UserName)).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByGroupId(Guid groupId)
        {
            return await _dbContext.Users
                .Include(u => u.GroupUsers)
                .Where(u => u.GroupUsers.Any(u => u.GroupId == groupId))
                .ToListAsync();
        }
    }
}
