using Microsoft.EntityFrameworkCore;
using ReenbitTest.Context;
using ReenbitTest.Entities;
using ReenbitTest.Services.Interfaces;

namespace ReenbitTest.Services
{
    public class GroupService : IGroupService
    {
        ChatContext _dbContext;

        public GroupService(ChatContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId)
        {
            return await _dbContext.Groups
                .Where(g => g.GroupUsers.Any(gu => gu.UserId == userId))
                .ToListAsync();
        }

        public async Task<Group> GetOrCreateGroup(string groupName)
        {
            var group = await GetGroupByGroupName(groupName);
            if (group == null)
            {
                group = new Group
                {
                    Name = groupName
                };

                await _dbContext.Groups.AddAsync(group);
                await _dbContext.SaveChangesAsync();
            }

            return group;
        }

        public async Task<Group> GetGroupByGroupName(string groupName)
        {
            return await _dbContext.Groups.FirstOrDefaultAsync(g => g.Name == groupName);
        }
    }
}