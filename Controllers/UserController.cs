using Microsoft.AspNetCore.Mvc;
using RascalChatApp.Models;
using RascalChatApp.Models.Request;
using RascalChatApp.Services;
using RascalChatApp.ViewModels;

namespace RascalChatApp.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        public readonly UserService userService;
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(string login, string password)
        {
            var registerResponse = userService.GetRegInfo(new UserCredentials { Login = login, Password = password });
            if (registerResponse.UserId == 0)
            {
                return RedirectToAction("registrationError", "home");
            }
            return Login(login, password);
        }

        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            var loginResponse = userService.GetLoginInfo(login, password);
            if (loginResponse == null)
            {
                return RedirectToAction("loginError", "home");
            }
            return RedirectToAction("profile", "user");
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var model = userService.GetUserInfo();
            var viewModel = new User { ApiKey = userService.GetCurrentApiKey(), Username = model.Username, UserId = model.UserId, AvatarUrl = model.AvatarUrl };
            return View(viewModel);
        }

        //[HttpGet("")]
        //public IActionResult UserInfo()
        //{
        //    var model = userService.GetUserInfo();
        //    return View(model);
        //}

        [HttpGet("update")]
        public IActionResult Update()
        {
            //var userModel = new User { Username = username };

            return View();
        }

        [HttpPost("update")]
        public IActionResult Update(string username, string avatarUrl)
        {
            var response = userService.UpdateUser(username, avatarUrl);
            var model = new UpdateConfirmViewModel { Username = username, AvatarUrl = avatarUrl, UserId = response.UserId };
            return View("updateConfirmation", model);
        }

        [HttpGet("updateConfirmation")]
        public IActionResult UpdateConfirmation(UpdateConfirmViewModel model)
        {
            return View(model);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            userService.Logout();
            return RedirectToAction("index", "home");
        }
    }
}