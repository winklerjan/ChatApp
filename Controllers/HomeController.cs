using Microsoft.AspNetCore.Mvc;
using RascalChatApp.Database;
using RascalChatApp.Models;
using RascalChatApp.Models.Request;
using RascalChatApp.Services;
using RascalChatApp.ViewModels;

namespace RascalChatApp.Controllers
{
    public class HomeController : Controller
    {
        public readonly ChannelService channelService;
        public readonly UserService userService;
        public readonly MessageService messageService;
        private readonly ApplicationDbContext dbContext;

        private readonly string apiKey;

        public HomeController(ChannelService channelService, UserService userService, MessageService messageService, ApplicationDbContext dbContext)
        {
            this.channelService = channelService;
            this.userService = userService;
            this.messageService = messageService;
            this.dbContext = dbContext;
            apiKey = userService.GetCurrentApiKey();
        }

        [HttpGet("")]
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("registrationError")]
        public IActionResult RegistrationError()
        {
            return View();
        }

        [HttpGet("loginError")]
        public IActionResult LoginError()
        {
            return View();
        }

        [HttpGet("retrieveChat")]
        public IActionResult RetrieveChat(MessagesRequest messagesToShow)
        {
            var data = channelService.GetAllMessages(messagesToShow, apiKey);
            var model = new ChatViewModel { Username = userService.GetUserInfo().Username, ApiKey = apiKey, Messages = data.Messages, Channel = data.Channel };
            return View(model);
        }

        [HttpPost("")]
        public IActionResult NewMessage(int channelId, /*string channelSecret,*/ string content)
        {
            string channelSecret = null;
            foreach (var ch in dbContext.Channels)
            {
                if (ch.Id == channelId)
                {
                    channelSecret = ch.Secret;
                }
            }
            var messagesToShow = new MessagesRequest { ChannelId = channelId, ChannelSecret = channelSecret, Count = 200 };
            var channel = new Channel { Id = channelId, Secret = channelSecret };
            messageService.PostMessage(new MessageRequest { ApiKey = userService.GetCurrentApiKey(), ChannelId = channelId, ChannelSecret = channelSecret, Content = content });
            var model = channelService.GetAllMessages(messagesToShow, apiKey);
            var viewModel = new ChatViewModel { Channel = channel, ApiKey = model.ApiKey, Messages = model.Messages, Username = userService.GetUserInfo().Username };

            return View("retrieveChat", viewModel);
        }
    }
}
