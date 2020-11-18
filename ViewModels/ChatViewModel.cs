using RascalChatApp.Models;
using System.Collections.Generic;

namespace RascalChatApp.ViewModels
{
    public class ChatViewModel
    {
        public int? ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public string Username { get; set; }
        public string ApiKey { get; set; }
        public List<Message> Messages { get; set; }
        public User Author { get; set; }
        public Channel Channel { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public Data Data { get; set; }
    }
}