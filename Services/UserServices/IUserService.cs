using System.Collections.Generic;
using System.Threading.Tasks;
using Inveasy.Models;

namespace Inveasy.Services.UserServices
{
    public interface IUserService
    {
        string StatusMessage { get; }
        Task<User> GetUserAsync(string username);
        Task<User> GetUserAsync(int id);
        Task<List<User>> GetUsersAsync();
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(string username, User updatedUser);
        Task<bool> UpdateUserAsync(int userId, User updatedUser);
        Task<bool> UpdateUserAsync(User userToUpdate, User updatedUser);
        Task<bool> DeleteUserAsync(string username);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> DeleteUserAsync(User userToDelete);
    }
}