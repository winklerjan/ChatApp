using System.Collections.Generic;

namespace RascalChatApp.Models.Response
{
    public class MessageResponse
    {
        public string Content { get; set; }
        public string Created { get; set; }
        public User Author { get; set; }
        public List<Message> Messages { get; set; }
    }
}
