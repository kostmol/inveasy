using Inveasy.Data;
using Inveasy.Models;
using Microsoft.EntityFrameworkCore;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.UserServices
{
    public class RoleService : IRoleService
    {
        private readonly InveasyContext _context;
        private readonly RoleStatus _status;

        public string StatusMessage { get; private set; } = "RoleService initialized";

        public RoleService(InveasyContext context, RoleStatus serviceStatus)
        {
            _context = context;
            _status = serviceStatus;
        }

        #region GET services
        // Method that return role from given role id
        public async Task<Role> GetRoleAsync(int roleId)
        {
            var roles = await GetRolesAsync();
            return roles?.FirstOrDefault(d => d.Id == roleId);
        }

        public async Task<Role> GetRoleAsync(string roleName)
        {
            var roles = await GetRolesAsync();
            return roles?.FirstOrDefault(d => d.Name == roleName);
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            try
            {
                return await _context.Role.ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

    }
}
