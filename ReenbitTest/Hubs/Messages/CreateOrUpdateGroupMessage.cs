namespace ReenbitTest.Hubs.Messages
{
    public class CreateOrUpdateGroupMessage
    {
        public string GroupName { get; set; }
        public IEnumerable<string> UserNames { get; set; }
    }
}
