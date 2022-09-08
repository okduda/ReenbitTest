using ReenbitTest.Entities;

namespace ReenbitTest.Services.Interfaces
{
    public interface IConnectionService
    {
        public Task AddConnection(string connectionId, Guid userId);
        public Task<IList<Connection>> GetConnectionsByUserId(Guid userId);
    }
}
