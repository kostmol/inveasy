using Inveasy.Data;
using Inveasy.Models;
using Microsoft.EntityFrameworkCore;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly InveasyContext _context;
        private readonly UserStatus _status;

        public string StatusMessage { get; private set; } = "UserService initialized";

        public UserService(InveasyContext context, UserStatus serviceStatus)
        {
            _context = context;
            _status = serviceStatus;
        }

        #region GET services
        // Method to retrieve user with given username
        public async Task<User> GetUserAsync(string username)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(d => d.Username == username);
        }

        // Method to retrieve user with given id
        public async Task<User> GetUserAsync(int id)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(d => d.Id == id);
        }

        // Method to retrieve all users 
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                return await _context.User
                ?.Include(u => u.Roles)
                ?.Include(i => i.Views)
                ?.Include(p => p.Comments).ThenInclude(o => o.Project)
                ?.Include(t => t.Donations).ThenInclude(q => q.Project)
                ?.Include(d => d.Image)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

        #region ADD services
        // Method to add a new user
        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null user");
                return false; // User already exists                                
            }

            try
            {
                // Check if user exists
                if (_context.User.Any(d => d.Id == user.Id))
                {
                    StatusMessage = _status.WarningAddStatus($"User {user.Username} already exists in database");
                    return true;
                }

                // Adding given user
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessAddStatus(user.Username);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionAddStatus(ex.Message, user.Username);
                return false;
            }
        }
        #endregion

        #region UPDATE/DELETE services
        // Method to update a user with a given username
        public async Task<bool> UpdateUserAsync(string username, User updatedUser) => await UpdateUserAsync(await GetUserAsync(username), updatedUser);

        // Method to update a user with a given username
        public async Task<bool> UpdateUserAsync(int userId, User updatedUser) => await UpdateUserAsync(await GetUserAsync(userId), updatedUser);

        public async Task<bool> UpdateUserAsync(User userToUpdate, User updatedUser)
        {
            // If user doesn't exist, do nothing
            if (userToUpdate == null || updatedUser == null)
            {
                StatusMessage = _status.ErrorUpdateStatus("Null user");
                return false;
            }

            try
            {
                // Check if user exists
                if (!_context.User.Any(d => d.Id == userToUpdate.Id))
                {
                    StatusMessage = _status.ErrorUpdateStatus("User doesn't exist in database");
                    return false;
                }

                List<string> updatedFields = new List<string>();

                // Update user and save changes
                if (updatedUser.Username != null)
                {
                    userToUpdate.Username = updatedUser.Username;
                    updatedFields.Add("username");
                }
                if (updatedUser.Password != null)
                {
                    userToUpdate.Password = updatedUser.Password;
                    updatedFields.Add("password");
                }
                if (updatedUser.Email != null)
                {
                    userToUpdate.Email = updatedUser.Email;
                    updatedFields.Add("email");
                }
                if (updatedUser.Name != null)
                {
                    userToUpdate.Name = updatedUser.Name;
                    updatedFields.Add("name");
                }
                if (updatedUser.Surname != null)
                {
                    userToUpdate.Surname = updatedUser.Surname;
                    updatedFields.Add("surname");
                }
                if (updatedUser.Birthday != default)
                {
                    userToUpdate.Birthday = updatedUser.Birthday;
                    updatedFields.Add("birthday");
                }
                if (updatedUser.Roles != null)
                {
                    userToUpdate.Roles = updatedUser.Roles;
                    updatedFields.Add("roles");
                }
                if (updatedUser.Image != null)
                {
                    userToUpdate.Image = updatedUser.Image;
                    updatedFields.Add("image");
                }

                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessUpdateStatus(userToUpdate.Username, updatedFields);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionUpdateStatus(ex.Message, userToUpdate.Username);
                return false;
            }
        }

        // Method to delete a user with a given username
        public async Task<bool> DeleteUserAsync(string username) => await DeleteUserAsync(await GetUserAsync(username));

        // Method to delete a user with a given username
        public async Task<bool> DeleteUserAsync(int userId) => await DeleteUserAsync(await GetUserAsync(userId));

        // Method to delete a user with a given username
        public async Task<bool> DeleteUserAsync(User userToDelete)
        {
            // If user doesn't exist, do nothing
            if (userToDelete == null)
            {
                StatusMessage = _status.ErrorDeleteStatus("Null user");
                return false;
            }

            try
            {
                // Check if user exists
                if (!_context.User.Any(d => d.Id == userToDelete.Id))
                {
                    StatusMessage = _status.WarningDeleteStatus("User doesn't exist in database");
                    return true;
                }

                // Update user and save changes                
                _context.User.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionDeleteStatus(ex.Message, userToDelete.Username);
                return false;
            }
        }
        #endregion
    }
}
