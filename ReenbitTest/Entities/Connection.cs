namespace ReenbitTest.Entities
{
    public class Connection : BaseEntity
    {
        public Connection() : base() { }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string ConnectionId { get; set; }
    }
}
