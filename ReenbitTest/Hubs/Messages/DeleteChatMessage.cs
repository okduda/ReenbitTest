namespace ReenbitTest.Hubs.Messages
{
    public class DeleteChatMessage
    {
        public Guid ChatMessageId { get; set; }
        public bool DeleteForAll { get; set; }
    }
}
