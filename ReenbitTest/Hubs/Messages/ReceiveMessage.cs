namespace ReenbitTest.Hubs.Messages
{
    public class ReceiveMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string GroupName { get; set; }
        public string UserName { get; set; }
    }
}
