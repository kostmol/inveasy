using Inveasy.Data;
using Inveasy.Models;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Microsoft.EntityFrameworkCore;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.ViewServices
{
    public class ViewService : IViewService
    {
        private readonly InveasyContext _context;
        private readonly ViewStatus _status;

        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public string StatusMessage { get; private set; } = "ViewService initialized";

        public ViewService(InveasyContext context, ViewStatus serviceStatus, IUserService userService, IProjectService projectService)
        {
            _context = context;
            _status = serviceStatus;
            _userService = userService;
            _projectService = projectService;
        }

        #region GET services
        // Method that gets views with given view id
        public async Task<View> GetViewAsync(int viewId)
        {
            var views = await GetViewsAsync();
            return views?.FirstOrDefault(d => d.Id == viewId);
        }

        // Method that returns views made by user from given username
        public async Task<List<View>> GetUserViewsAsync(string username) => await GetUserViewsAsync(await _userService.GetUserAsync(username));

        // Method that returns views made by user from given user id
        public async Task<List<View>> GetUserViewsAsync(int userId) => await GetUserViewsAsync(await _userService.GetUserAsync(userId));

        // Method that returns views made by user from given user 
        public async Task<List<View>> GetUserViewsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                StatusMessage = _status.ErrorGetStatus("Null user");
                return null;
            }

            var views = await GetViewsAsync();
            return views?.Where(d => d.User.Id == user.Id).ToList();
        }

        public async Task<List<View>> GetViewsAsync()
        {
            try
            {
                return await _context.View
                    ?.Include(p => p.Project)
                    ?.Include(s => s.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

        #region View ADD services
        public async Task<bool> AddViewAsync(Project project, int userId)
            => await AddViewAsync(project, await _userService.GetUserAsync(userId));

        public async Task<bool> AddViewAsync(Project project, string username)
            => await AddViewAsync(project, await _userService.GetUserAsync(username));

        public async Task<bool> AddViewAsync(int projectId, User user)
            => await AddViewAsync(await _projectService.GetProjectAsync(projectId), user);

        public async Task<bool> AddViewAsync(int projectId, string username)
            => await AddViewAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(username));

        public async Task<bool> AddViewAsync(int projectId, int userId)
            => await AddViewAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(userId));

        public async Task<bool> AddViewAsync(Project project, User user)
        {
            if (project == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null project");
                return false;
            }

            if (user == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null user");
                return false;
            }

            // Create View
            View view = new View
            {
                Date = DateTime.Now,
                User = user,
                Project = project
            };

            var viewAdded = await AddViewAsync(view);

            if (!viewAdded)
                return false;

            return true;
        }

        public async Task<bool> AddViewAsync(View view)
        {
            try
            {
                // Check if view exists
                if (_context.View.Any(d => d.Id == view.Id))
                {
                    StatusMessage = _status.WarningGetStatus("View already exists in database");
                    return true;
                }

                // Store View
                _context.View.AddAsync(view);
                await _context.SaveChangesAsync();
                _status.SuccessAddStatus(view.Id.ToString());
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionAddStatus(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
