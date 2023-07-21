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
using System.Reflection;
using System.Reflection.Metadata;

namespace Inveasy.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;        
        private List<string> _properties = new List<string>();

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
            _properties = GetProperties();
        }

        #region Endpoints
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetUserById/");
            try
            {
                var user = await _userService.GetUserAsync(userId);
                return await GetUserAsResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserByUsername/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetUserByUsername/");
            try
            {
                var user = await _userService.GetUserAsync(username);
                return await GetUserAsResponseAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserViews/{username}")]
        public async Task<IActionResult> GetUserViewsByUsername(string username)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetViewsByUsername/");
            try
            {
                var user = await _userService.GetUserAsync(username);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Username" || x == "Views");

                var JUser = GetUserAsJObject(user, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JUser);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserComments/{username}")]
        public async Task<IActionResult> GetUserCommentsByUsername(string username)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetCommentsByUsername/");
            try
            {
                var user = await _userService.GetUserAsync(username);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Username" || x == "Comments");

                var JUser = GetUserAsJObject(user, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JUser);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserDonations/{username}")]
        public async Task<IActionResult> GetUserDonationsByUsername(string username)
        {
            _logger.LogInformation($"Someone made a GET request at /api/User/GetDonationsByUsername/");
            try
            {
                var user = await _userService.GetUserAsync(username);

                var propertiesToNotSend = _properties;
                propertiesToNotSend.RemoveAll(x => x == "Username" || x == "Donations");

                var JUser = GetUserAsJObject(user, propertiesToNotSend);

                var json = JsonConvert.SerializeObject(JUser);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception was raised while trying to retrieve and send information for a user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        private async Task<IActionResult> GetUserAsResponseAsync(User user)
        {
            if (user == null) return Ok("Non valid user given");

            var propertiesToNotSend = _properties;
            propertiesToNotSend.RemoveAll(x => x == "Id" || x == "Username" || x == "Name" || x == "Surname" || x == "Email" || x == "Birthday" || x == "Roles");

            var JUser = GetUserAsJObject(user, propertiesToNotSend);

            var json = JsonConvert.SerializeObject(JUser);
            return Content(json, "application/json");            
        }

        private JObject GetUserAsJObject(User user, List<string> removedProperties)
        {
            JObject jUser = JObject.FromObject(user);

            if (jUser == null) { return null; }

            _logger.LogInformation($"Someone requested information for user {jUser.Property("Username").Value}");

            foreach (var property in removedProperties)
            {
                if (jUser.ContainsKey(property))
                    jUser.Property(property).Remove();
            }

            return jUser;
            
        }

        private static List<string> GetProperties()
        {
            PropertyInfo[] properties = typeof(User).GetProperties();

            List<string> propertyNames = new List<string>();
            foreach (PropertyInfo property in properties)
            {
                propertyNames.Add(property.Name);
            }

            return propertyNames;
        }
    }
}

