using Inveasy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inveasy.Services.ProjectServices
{
    public interface IProjectService
    {
        public string StatusMessage { get; }

        Task<List<Project>> GetProjectsAsync(string projectName);
        Task<Project> GetProjectAsync(int projectId);
        Task<List<Project>> GetUserProjectsAsync(string username);
        Task<List<Project>> GetUserProjectsAsync(int userId);
        Task<List<Project>> GetUserProjectsAsync(User user);
        Task<List<Project>> GetProjectsAsync();
        Task<bool> AddProjectAsync(Project project);
        Task<bool> UpdateProjectAsync(int projectToUpdateId, Project updatedProject);
        Task<bool> UpdateProjectAsync(Project projectToUpdate, Project updatedProject);
        Task<bool> DeleteProjectAsync(int projectId);
        Task<bool> DeleteProjectAsync(Project projectToDelete);
    }
}