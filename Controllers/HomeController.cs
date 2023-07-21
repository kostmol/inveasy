using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;

namespace Inveasy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IProjectService projectService, IUserService userService)
        {
            _logger = logger;
            _projectService = projectService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetProjectsAsync();
            var users = await _userService.GetUsersAsync();
            
            // users.Count(u => u.Roles in ())
            var totalMoney = projects.Sum(p => p.FundAmount);
            var pFunded = projects.Count(p => p.FundAmount > 0);
            var backers = users.Count(u => u.Roles.Any(r => r.Name == "Backer"));
            
            ViewData["totalMoney"] = totalMoney;
            ViewData["pFunded"] = pFunded;
            ViewData["backers"] = backers;
            
            return View();
        }
        public IActionResult Project()
        {
            return View();
        }

        public IActionResult Category()
        {
            return View();
        }

        public IActionResult Popularity()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}