using Newtonsoft.Json;
using RascalChatApp.Models.Request;
using RascalChatApp.Models.Response;
using System.Net.Http;
using System.Text;

namespace RascalChatApp.Services
{
    public class MessageService
    {
        public readonly IHttpClientFactory clientFactory;
        public MessageService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public MessageResponse PostMessage(MessageRequest messageRequest)
        {
            var messageInfo = JsonConvert.SerializeObject(messageRequest);
            var httpContent = new StringContent(messageInfo, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", messageRequest.ApiKey);

            string endpointUrl = "https://latest-chat.herokuapp.com/api/message";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var messageResponse = JsonConvert.DeserializeObject<MessageResponse>(responseContent);
            return messageResponse;
        }
    }
}
