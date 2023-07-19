using Inveasy.Models;

namespace Inveasy.Services.UserServices
{
    public interface IRoleService
    {
        string StatusMessage { get; }

        Task<Role> GetRoleAsync(int roleId);
        Task<Role> GetRoleAsync(string roleName);
        Task<List<Role>> GetRolesAsync();
    }
}
