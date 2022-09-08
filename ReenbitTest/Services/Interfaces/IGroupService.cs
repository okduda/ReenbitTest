using ReenbitTest.Entities;

namespace ReenbitTest.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId);
        public Task<Group> GetOrCreateGroup(string groupName);
        public Task<Group> GetGroupByGroupName(string groupName);
    }
}
