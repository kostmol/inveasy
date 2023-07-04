using Inveasy.Data;
using Inveasy.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveasy.Services
{
    public class UsersService
    {
        private readonly InveasyContext _context;

        public UsersService(InveasyContext context)
        {
            _context = context;
        }

        // Method to retrieve user with given username
        public async Task<User> GetUser(string username) => await _context.User.FirstOrDefaultAsync(d => d.Username == username);

        // Method to add a new user
        public async Task<bool> AddUser(User user)
        {
            if (GetUser(user.Username) == null) 
            {
                return false; // User doesn't exist
            }

            try
            {
                // Adding given user
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            { 
                return false;
            }
        }

        // Method to update a user with a given username
        public async Task<bool> UpdateUser(string username, User updatedUser)
        {
            // Get user            
            User userToUpdate = await GetUser(username);

            // If user doesn't exist, do nothing
            if (userToUpdate == null)            
                return false;

            try 
            {
                // Update user and save changes
                userToUpdate = updatedUser;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        // Method to delete a user with a given username
        public async Task<bool> DeleteUser(string username)
        {
            // Get user
            User userToDelete = await GetUser(username);

            // If user doesn't exist, do nothing
            if (userToDelete == null)
                return false;

            try
            {
                // Update user and save changes                
                _context.User.Remove(userToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        



    }
}
