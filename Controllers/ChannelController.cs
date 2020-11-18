using Microsoft.AspNetCore.Mvc;
using RascalChatApp.Models.Request;
using RascalChatApp.Services;
using RascalChatApp.ViewModels;

namespace RascalChatApp.Controllers
{
    [Route("api/channel")]
    public class ChannelController : Controller
    {
        public readonly ChannelService channelService;
        public readonly UserService userService;
        private readonly string apiKey;
        public ChannelController(ChannelService channelService, UserService userService)
        {
            this.channelService = channelService;
            this.userService = userService;
            apiKey = userService.GetCurrentApiKey();
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(string name, string description)
        {
            channelService.CreateChannel(name, description/*, apiKey*/);
            return RedirectToAction("userChannels");
        }

        [HttpPost("add")]
        public IActionResult Add(string name, string description, int channelId, string channelSecret)
        {
            channelService.AddChannel(name, description, channelId, channelSecret);
            return RedirectToAction("profile", "user");
        }


        [HttpGet("user-channels")]
        public IActionResult UserChannels()
        {
            var model = new UserChannelsViewModel { Channels = channelService.GetAllApiChannels(apiKey) };
            return View(model);
        }

        [HttpGet("localChannels")]
        public IActionResult LocalChannels()
        {
            var model = new UserChannelsViewModel { Channels = channelService.GetAllLocalChannels() };
            return View(model);
        }

        [HttpGet("updateChannel")]
        public IActionResult UpdateChannel(UpdateChannelRequest toUpdate)
        {
            return View(toUpdate);
        }

        [HttpPost("update")]
        public IActionResult Update(UpdateChannelRequest toUpdate)
        {
            channelService.UpdateChannel(apiKey, toUpdate);
            return RedirectToAction("userChannels");
        }

        [HttpPost("get-messages")]
        public IActionResult GetMessages(MessagesRequest messagesToShow)
        {
            var model = channelService.GetAllMessages(messagesToShow, apiKey);
            return RedirectToAction("retrieveChat", "home", model);
        }
    }
}