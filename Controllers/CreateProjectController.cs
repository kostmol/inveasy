using Inveasy.Models;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Inveasy.Controllers;

public class CreateProjectController : Controller
{
    private readonly ILogger<CreateProjectController> _logger;
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;

    public CreateProjectController(ILogger<CreateProjectController> logger,
                                    ICategoryService categoryService,
                                    IProjectService projectService,
                                    IUserService userService)
    {
        _logger = logger;
        _categoryService = categoryService;
        _projectService = projectService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["categories"] = await _categoryService.GetCategoriesAsync();
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Creates(Project project, [Bind("CategoryId")] int categoryId)
    {
        var session = HttpContext.Session.GetString("User");
        var json = JObject.Parse(session);
        
        var username = json["Username"].ToString();

        var user = await _userService.GetUserAsync(username);

        var category = await _categoryService.GetCategoryAsync(categoryId);
        project.Categories = new List<Category>();
        project.Categories.Add(category);
        
        project.User = user;
        
        await _projectService.AddProjectAsync(project);
        
        return RedirectToAction("Index", "UserProjects");
    }
}