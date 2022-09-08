using ReenbitTest.Entities;

namespace ReenbitTest.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetOrCreateUser(string userName);
        public Task<User> GetUserByConnectionId(string connectionId);
        public Task<IEnumerable<User>> GetUsersByUserNames(IEnumerable<string> userNames);
        public Task<IEnumerable<User>> GetUsersByGroupId(Guid groupId);
    }
}
