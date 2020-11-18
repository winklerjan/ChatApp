using System.Collections.Generic;

namespace RascalChatApp.Models
{
    public class Data
    {
        public string ApiKey { get; set; }
        public List<Message> Messages { get; set; }
        public Channel Channel { get; set; }
    }
}
