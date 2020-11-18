namespace RascalChatApp.Models
{
    public class Message
    {
        public int? Id { get; set; }
        public int? ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public User Author { get; set; }
        public Channel Channel { get; set; }
        public int? Count { get; set; }
    }
}
