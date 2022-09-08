namespace ReenbitTest.Entities
{
    public class Group : BaseEntity
    {
        public Group() : base() { }

        public string Name { get; set; }
        public IEnumerable<GroupUser> GroupUsers { get; set; }
    }
}
