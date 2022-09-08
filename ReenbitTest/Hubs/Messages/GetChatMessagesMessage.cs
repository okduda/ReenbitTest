namespace ReenbitTest.Hubs.Messages
{
    public class GetChatMessagesMessage
    {
        public int Page { get; set; }
        public string GroupName { get; set; }
    }
}
