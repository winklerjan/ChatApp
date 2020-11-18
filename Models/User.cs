namespace RascalChatApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
