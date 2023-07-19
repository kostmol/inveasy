using System.Text.Json;
using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using Inveasy.Services.UserServices;

namespace Inveasy.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var authUser = await _userService.GetUserAsync(user.Username);
            if (authUser != null)
            {
                HttpContext.Session.SetString("User", JsonSerializer.Serialize(authUser));
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError("", "Invalid login attempt");
            return RedirectToAction("Index");
        }
    }
}
