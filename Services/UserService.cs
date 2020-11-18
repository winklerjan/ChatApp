using Newtonsoft.Json;
using RascalChatApp.Models.Request;
using RascalChatApp.Models.Response;
using System.IO;
using System.Net.Http;
using System.Text;

namespace RascalChatApp.Services
{
    public class UserService
    {
        public string CurrentApiKeyStorage = @"C:\Users\janwi\Desktop\GREENFOX\winklerjan\week-10\RascalChatApp\RascalChatApp\CurrentApiKey.txt";

        public readonly IHttpClientFactory clientFactory;
        public UserService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public UserInfoResponse GetRegInfo(UserCredentials credentials)
        {
            var loginCredentials = JsonConvert.SerializeObject(credentials);
            var httpContent = new StringContent(loginCredentials, Encoding.UTF8, "application/json");

            string endpointUrl = "https://latest-chat.herokuapp.com/api/user/register";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            if (responseContent == "Login already taken.")
            {
                return new UserInfoResponse();
            }
            var regInfo = JsonConvert.DeserializeObject<UserInfoResponse>(responseContent);
            return regInfo;
        }

        public LoginResponse GetLoginInfo(string login, string password)
        {
            var loginCredentials = JsonConvert.SerializeObject(new UserCredentials { Login = login, Password = password });
            var httpContent = new StringContent(loginCredentials, Encoding.UTF8, "application/json");

            string endpointUrl = "https://latest-chat.herokuapp.com/api/user/login";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            LoginResponse loginInfo = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
            SaveCurrentApiKey(loginInfo);

            return loginInfo;
        }

        public UserInfoResponse GetUserInfo()
        {
            string endpointUrl = "https://latest-chat.herokuapp.com/api/user/";

            var httpContent = clientFactory.CreateClient();
            httpContent.DefaultRequestHeaders.Add("apiKey", GetCurrentApiKey());

            var response = httpContent.GetAsync(endpointUrl).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(responseContent);
            return userInfo;
        }

        public void SaveCurrentApiKey(LoginResponse response)
        {
            File.WriteAllText(CurrentApiKeyStorage, response.ApiKey);
        }

        public string GetCurrentApiKey()
        {
            return File.ReadAllText(CurrentApiKeyStorage);
        }

        public UpdateResponse UpdateUser(string username, string avatarUrl)
        {
            var updateRequest = JsonConvert.SerializeObject(new UpdateUserRequest { Username = username, AvatarUrl = avatarUrl });
            var httpContent = new StringContent(updateRequest, Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", GetCurrentApiKey());

            string endpointUrl = "https://latest-chat.herokuapp.com/api/user/update";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            UpdateResponse updatedUserInfo = JsonConvert.DeserializeObject<UpdateResponse>(responseContent);
            return updatedUserInfo;
        }

        public bool Logout()
        {
            var httpContent = new StringContent(GetCurrentApiKey(), Encoding.UTF8, "application/json");
            httpContent.Headers.Add("apiKey", GetCurrentApiKey());

            string endpointUrl = "https://latest-chat.herokuapp.com/api/user/logout";

            var response = clientFactory.CreateClient().PostAsync(endpointUrl, httpContent).Result;
            var responseContent = response.Content.ReadAsStringAsync().Result;

            var userLoggedOut = JsonConvert.DeserializeObject<bool>(responseContent);

            File.WriteAllText(CurrentApiKeyStorage, "");

            return userLoggedOut;
        }
    }
}