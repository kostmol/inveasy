using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Newtonsoft.Json.Linq;

namespace Inveasy.Controllers
{
    public class UserProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public UserProjectsController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }
            
        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("User");
            var json = JObject.Parse(session);
            var username = json["Username"].ToString();

            var user = await _userService.GetUserAsync(username);

            var projects = await _projectService.GetUserProjectsAsync(user);
            ViewData["projects"] = projects;
            return View(projects);
        }
    }
}
