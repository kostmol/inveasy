using Microsoft.AspNetCore.Mvc;
using Inveasy.Services.ProjectServices;
namespace Inveasy.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectService _projectService;
        public ProjectController(ILogger<ProjectController> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var project = await _projectService.GetProjectAsync(id);
            return View(project);
        }
    }
}