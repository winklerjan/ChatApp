using Newtonsoft.Json;
using RascalChatApp.Database;
using RascalChatApp.Models;
using RascalChatApp.Models.Request;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace RascalChatApp.Services
{
    public class ChannelService
    {
        private readonly ApplicationDbContext dbContext;
        public readonly UserService userService;
        public readonly IHttpClientFactory clientFactory;
        private readonly string apiKey;

        public ChannelService(IHttpClientFactory clientFactory, UserService userService, ApplicationDbContext dbContext)
        {
            this.clientFactory = clientFactory;
            this.dbContext = dbContext;
            apiKey = userService.GetCurrentApiKey();
        }

        public Channel CreateChannel(string name, string description/*, string apiKey*/)
        {
            var basicInfo = JsonConvert.SerializeObject(new ChannelRequest { Name = name, Description = description });
            var httpContent = new StringContent(basicInfo, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", apiKey);

            string endpointUrl = "https://latest-chat.herokuapp.com/api/channel";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Channel channelInfo = JsonConvert.DeserializeObject<Channel>(responseContent);

            dbContext.Add(channelInfo);
            dbContext.SaveChanges();

            return channelInfo;
        }

        public void AddChannel(string name, string description, int channelId, string channelSecret)
        {
            var channel = new Channel { Name = name, Description = description, Id = channelId, Secret = channelSecret };

            dbContext.Channels.Add(channel);
            dbContext.SaveChanges();
        }

        public List<Channel> GetAllApiChannels(string apiKey)
        {
            string endpointUrl = "https://latest-chat.herokuapp.com/api/channel/user-channels";

            var httpContent = clientFactory.CreateClient();
            httpContent.DefaultRequestHeaders.Add("apiKey", apiKey);

            var response = httpContent.GetAsync(endpointUrl).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var channels = JsonConvert.DeserializeObject<List<Channel>>(responseContent);

            foreach (var ch in channels)
            {
                if (!dbContext.Channels.Any(c => c.Id == ch.Id))
                {
                    dbContext.Channels.Add(ch);
                }
            }
            dbContext.SaveChanges();
            return channels;
        }

        public List<Channel> GetAllLocalChannels()
        {
            return dbContext.Channels.ToList();
        }

        public Channel UpdateChannel(string apiKey, UpdateChannelRequest updateRequest)
        {
            var toUpdate = JsonConvert.SerializeObject(updateRequest);
            var httpContent = new StringContent(toUpdate, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", apiKey);

            string endpointUrl = "https://latest-chat.herokuapp.com/api/channel/update";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var channelInfo = JsonConvert.DeserializeObject<Channel>(responseContent);
            return channelInfo;
        }

        public Data GetAllMessages(MessagesRequest messagesRequest, string apiKey)
        {
            var messagesToShow = JsonConvert.SerializeObject(messagesRequest);
            var httpContent = new StringContent(messagesToShow, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", apiKey);

            string endpointUrl = "https://latest-chat.herokuapp.com/api/channel/get-messages";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var messages = JsonConvert.DeserializeObject<Data>(responseContent);
            return messages;
        }
    }
}