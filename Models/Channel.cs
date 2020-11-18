using System.ComponentModel.DataAnnotations;

namespace RascalChatApp.Models
{
    public class Channel
    {
        [Key]
        public int LocalId { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string Admin { get; set; }
        public string Secret { get; set; }
    }
}
