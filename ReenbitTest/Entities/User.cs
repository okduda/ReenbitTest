namespace ReenbitTest.Entities
{
    public class User : BaseEntity
    {
        public User() : base() { }

        public string UserName { get; set; }

        public IEnumerable<Connection> Connections { get; set; }
        public IEnumerable<GroupUser> GroupUsers { get; set; }
    }
}
