namespace ReenbitTest.Entities
{
    public class GroupUser : BaseEntity
    {
        public GroupUser() : base() { }

        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
