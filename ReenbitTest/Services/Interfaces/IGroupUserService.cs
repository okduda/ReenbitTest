namespace ReenbitTest.Services.Interfaces
{
    public interface IGroupUserService
    {
        public Task AddUserToGroup(Guid userId, Guid groupId);
    }
}
