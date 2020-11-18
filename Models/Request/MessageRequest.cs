namespace RascalChatApp.Models.Request
{
    public class MessageRequest
    {
        public int ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public string Content { get; set; }
        public string ApiKey { get; set; }
    }
}
