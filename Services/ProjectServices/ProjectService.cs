using Inveasy.Data;
using Inveasy.Models;
using Inveasy.Services.UserServices;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.ProjectServices
{
    public class ProjectService : IProjectService
    {
        private readonly InveasyContext _context;
        private readonly ProjectStatus _status;

        private readonly IUserService _userService;

        public string StatusMessage { get; private set; } = "ProjectService initialized";

        public ProjectService(InveasyContext context, ProjectStatus serviceStatus, IUserService userService)
        {
            _context = context;
            _status = serviceStatus;
            _userService = userService;
        }

        #region GET services               
        // Method that returns list of project with given project name
        public async Task<List<Project>> GetProjectsAsync(string projectName)
        {
            var projects = await GetProjectsAsync();
            return projects?.Where(q => q.Name == projectName).ToList();
        }

        // Method that returns a project with the given project id
        public async Task<Project> GetProjectAsync(int projectId)
        {
            var projects = await GetProjectsAsync();
            return projects?.FirstOrDefault(q => q.Id == projectId);
        }

        // Method that returns all projects started by user from given username
        public async Task<List<Project>> GetUserProjectsAsync(string username) => await GetUserProjectsAsync(await _userService.GetUserAsync(username));

        // Method that returns all projects started by user from given user id
        public async Task<List<Project>> GetUserProjectsAsync(int userId) => await GetUserProjectsAsync(await _userService.GetUserAsync(userId));

        // Method that returns all projects started by user from given user 
        public async Task<List<Project>> GetUserProjectsAsync(User user)
        {
            if (user == null)
            {
                StatusMessage = _status.ErrorGetStatus("Null user");
                return null;
            }

            // If user doesn't have role of Project creator, do nothing
            if (!user.Roles.Any(d => d.Id == 3))
            {
                StatusMessage = _status.ErrorGetStatus("Given user does not have role of project creator");
                return null;
            }

            var projects = await GetProjectsAsync();
            return projects?.Where(d => d.User.Id == user.Id).ToList();
        }

        // Method that returns list of all projects
        public async Task<List<Project>> GetProjectsAsync()
        {
            try
            {
                return await _context.Project
                    ?.Include(t => t.RewardsTier)
                    ?.Include(u => u.User)
                    ?.Include(i => i.Views).ThenInclude(ο => ο.User)
                    ?.Include(p => p.Comments).ThenInclude(x => x.User)
                    ?.Include(t => t.Donations).ThenInclude(z => z.User)
                    ?.Include(u => u.Images)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

        #region Project ADD services
        // Method that adds project to db
        public async Task<bool> AddProjectAsync(Project project)
        {
            if (project == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null project");
                return false;
            }

            try
            {
                // Check if project exists
                if (_context.Project.Any(d => d.Id == project.Id))
                {
                    StatusMessage = _status.WarningAddStatus($"Project {project.Name} already exists in database");
                    return true;
                }

                await _context.Project.AddAsync(project);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessAddStatus(project.Name);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionAddStatus(StatusMessage, project.Name);
                return false;
            }
        }
        #endregion

        #region UPDATE/DELETE services
        // Method that updates given project
        public async Task<bool> UpdateProjectAsync(int projectToUpdateId, Project updatedProject) => await UpdateProjectAsync(await GetProjectAsync(projectToUpdateId), updatedProject);

        // Method that updates project with given id
        public async Task<bool> UpdateProjectAsync(Project projectToUpdate, Project updatedProject)
        {
            if (projectToUpdate == null || updatedProject == null)
            {
                StatusMessage = _status.ErrorUpdateStatus("Null project");
                return false;
            }

            try
            {
                // Check if project exists
                if (!_context.Project.Any(d => d.Id == projectToUpdate.Id))
                {
                    StatusMessage = _status.ErrorUpdateStatus("Project doesn't exist in database");
                    return false;
                }

                List<string> updatedFields = new List<string>();

                // Update project and save changes                
                if (updatedProject.Name != null)
                {
                    projectToUpdate.Name = updatedProject.Name;
                    updatedFields.Add("name");
                }
                if (updatedProject.Description != null)
                {
                    projectToUpdate.Description = updatedProject.Description;
                    updatedFields.Add("description");
                }
                if (updatedProject.FundAmount != 0)
                {
                    projectToUpdate.FundAmount = updatedProject.FundAmount;
                    updatedFields.Add("fundAmount");
                }
                if (updatedProject.FundGoal != null)
                {
                    projectToUpdate.FundGoal = updatedProject.FundGoal;
                    updatedFields.Add("fundGoal");
                }
                if (updatedProject.TrendingScore != null)
                {
                    projectToUpdate.TrendingScore = updatedProject.TrendingScore;
                    updatedFields.Add("trendingScore");
                }
                if (updatedProject.RewardsTier != null)
                {
                    projectToUpdate.RewardsTier = updatedProject.RewardsTier;
                    updatedFields.Add("rewardTier");
                }
                if (updatedProject.Categories != null)
                {
                    projectToUpdate.Categories = updatedProject.Categories;
                    updatedFields.Add("categories");
                }
                if (updatedProject.Images != null)
                {
                    projectToUpdate.Images = updatedProject.Images;
                    updatedFields.Add("images");
                }

                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessUpdateStatus(projectToUpdate.Name, updatedFields);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionUpdateStatus(ex.Message, projectToUpdate.Name);
                return false;
            }
        }

        // Method that removes project with given id
        public async Task<bool> DeleteProjectAsync(int projectId) => await DeleteProjectAsync(await GetProjectAsync(projectId));

        // Method that removes given project 
        public async Task<bool> DeleteProjectAsync(Project projectToDelete)
        {
            if (projectToDelete == null)
            {
                StatusMessage = _status.ErrorDeleteStatus("Null project");
                return false;
            }

            try
            {
                // Check if project exists
                if (!_context.Project.Any(d => d.Id == projectToDelete.Id))
                {
                    StatusMessage = _status.WarningDeleteStatus("Project doesn't exist in database");
                    return true;
                }

                // Update project and save changes
                _context.Project.Remove(projectToDelete);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessDeleteStatus(projectToDelete.Name);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionDeleteStatus(ex.Message, projectToDelete.Name);
                return false;
            }
        }
        #endregion
    }
}
