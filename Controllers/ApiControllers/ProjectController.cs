using Azure.Core;
using Humanizer;
using Inveasy.Models;
using Inveasy.Services.CommentServices;
using Inveasy.Services.DonationServices;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Inveasy.Services.ViewServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Reflection;
using System.Reflection.Metadata;

namespace Inveasy.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private List<string> _properties = new List<string>();

        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
            _properties = GetProperties();
        }

        #region Endpoints
        [HttpGet("GetProjectsByName/{name}")]
        public async Task<IActionResult> GetProjectsByName(string name)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetProjectByName/");
            try
            {
                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Id" || x == "Name" || x == "Description" || x == "CreatedDate" || x=="Categories");

                var projects = await _projectService.GetProjectsAsync(name);
                List<JObject> JProjects = new List<JObject>();

                foreach (var project in projects)
                {
                    JProjects.Add(GetProjectAsJObject(project, propertiesToNotSend));
                }

                var json = JsonConvert.SerializeObject(JProjects);

                return Content(json, "application/json");

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a project: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }        

        [HttpGet("GetProjectById/{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetProjectById/");
            try
            {
                var user = await _projectService.GetProjectAsync(projectId);
                return await GetProjectAsResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a project: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProjectViews/{projectId}")]
        public async Task<IActionResult> GetProjectViews(int projectId)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetProjectViews/");
            try
            {
                var project = await _projectService.GetProjectAsync(projectId);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Name" || x == "Views");

                var JProject = GetProjectAsJObject(project, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JProject);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a project: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProjectComments/{projectId}")]
        public async Task<IActionResult> GetProjectComments(int projectId)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetProjectComments/");
            try
            {
                var project = await _projectService.GetProjectAsync(projectId);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Name" || x == "Comments");

                var JProject = GetProjectAsJObject(project, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JProject);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a project: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProjectDonations/{projectId}")]
        public async Task<IActionResult> GetProjectDonations(int projectId)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetProjectDonations/");
            try
            {
                var project = await _projectService.GetProjectAsync(projectId);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Name" || x == "Donations");

                var JProject = GetProjectAsJObject(project, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JProject);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a project: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        private async Task<IActionResult> GetProjectAsResponseAsync(Project project)
        {
            if (project == null) return Ok("Non valid project given");

            var propertiesToNotSend = _properties;
            propertiesToNotSend.RemoveAll(x => x == "Id" || x == "Name" || x == "Description" || x == "CreatedDate" || x == "Categories");

            var JProject = GetProjectAsJObject(project, propertiesToNotSend);
            var json = JsonConvert.SerializeObject(JProject);            

            return Content(json, "application/json");
        }

        private JObject GetProjectAsJObject(Project project, List<string> removedProperties)
        {
            JObject jProject = JObject.FromObject(project);

            if (jProject == null) { return null; }

            _logger.LogInformation($"Someone requested information for user {jProject.Property("Name").Value}");

            foreach (var property in removedProperties)
            {
                if (jProject.ContainsKey(property))
                    jProject.Property(property).Remove();
            }

            return jProject;
        }

        private static List<string> GetProperties()
        {
            PropertyInfo[] properties = typeof(Project).GetProperties();

            List<string> propertyNames = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                propertyNames.Add(property.Name);
            }

            return propertyNames;
        }
    }
}
