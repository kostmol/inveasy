using Inveasy.Models;

namespace Inveasy.Services.ViewServices
{
    public interface IViewService
    {
        string StatusMessage { get; }

        Task<View> GetViewAsync(int viewId);
        Task<List<View>> GetUserViewsAsync(string username);
        Task<List<View>> GetUserViewsAsync(int userId);
        Task<List<View>> GetUserViewsAsync(User user);
        Task<List<View>> GetViewsAsync();
        Task<bool> AddViewAsync(Project project, int userId);
        Task<bool> AddViewAsync(Project project, string username);
        Task<bool> AddViewAsync(int projectId, User user);
        Task<bool> AddViewAsync(int projectId, string username);
        Task<bool> AddViewAsync(int projectId, int userId);
        Task<bool> AddViewAsync(Project project, User user);
        Task<bool> AddViewAsync(View view);
    }
}
