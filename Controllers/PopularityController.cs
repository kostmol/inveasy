using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Inveasy.Services.ProjectServices;

namespace Inveasy.Controllers
{
    public class PopularityController : Controller
    {
        private readonly IProjectService _projectService;

        public PopularityController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetProjectsAsync();
            ViewData["projects"] = projects;
            return View(projects);
        }
    }
}
