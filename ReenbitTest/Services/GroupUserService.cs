using ReenbitTest.Context;
using ReenbitTest.Entities;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Services
{
    public class GroupUserService : IGroupUserService
    {
        ChatContext _dbContext;

        public GroupUserService(ChatContext context)
        {
            _dbContext = context;
        }

        public async Task AddUserToGroup(Guid userId, Guid groupId)
        {
            var groupUser = new GroupUser
            {
                GroupId = groupId,
                UserId = userId
            };

            await _dbContext.GroupUsers.AddAsync(groupUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
