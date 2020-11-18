namespace RascalChatApp.Models.Request
{
    public class MessagesRequest
    {
        public int ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public int Count { get; set; }
    }
}
