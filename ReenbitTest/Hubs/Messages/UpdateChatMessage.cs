namespace ReenbitTest.Hubs.Messages
{
    public class UpdateChatMessage
    {
        public Guid ChatMessageId { get; set; }
        public string NewText { get; set; }
    }
}
