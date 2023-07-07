using Inveasy.Data;
using Inveasy.Models;
using Microsoft.EntityFrameworkCore;

namespace Inveasy.Services
{
    public class DatabaseService
    {
        private readonly InveasyContext _context;                

        public DatabaseService(InveasyContext context)
        {
            _context = context;
        }

//----- User services ----- //

        // Method to retrieve user with given username
        public async Task<User> GetUser(string username) => await _context.User.FirstOrDefaultAsync(d => d.Username == username);               
        
        // Method to add a new user
        public async Task<bool> AddUser(User user)
        {
            if (GetUser(user.Username) != null)
            {                
                return false; // User already exists                                
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

        // Method that adds role with given role id to user with given username
        public async Task<bool> AddRoleToUser(int roleId, string username)
        {
            // Get user
            User user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return false;

            // If user already has given role, return true
            if (user.Roles.Select(d => d.Id == roleId).Any())
                return true;

            try
            {
                Role desiredRole = await _context.Role.FirstOrDefaultAsync(d => d.Id == roleId);               
                if (desiredRole == null) return false; // Role with given id doesn't exist

                user.Roles.Add(desiredRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }        

        // Method that adds role with given role name to user with given username
        public async Task<bool> AddRoleToUser(string roleName, string username)
        {
            // Get user
            User user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return false;

            // If user already has given role, return true
            if (user.Roles.Select(d => d.Name == roleName).Any())
                return true;

            try
            {
                Role desiredRole = await _context.Role.FirstOrDefaultAsync(d => d.Name == roleName);
                if (desiredRole == null) return false; // Role with given name doesn't exist

                user.Roles.Add(desiredRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }        

    //----- Project services ----- //
        public async Task<Project> GetProject(int id) => await _context.Project.FirstOrDefaultAsync(p => p.Id == id);               
        public async Task<Project> GetProjects(User user) => await _context.Project.FirstOrDefaultAsync(p => p.User == user);               
        
        public async Task<bool> AddProject(Project project)
        {
            try
            {
                await _context.Project.AddAsync(project);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateProject(int id, Project updateProject)
        {
            var projectToUpdate = await GetProject(id);
            
            if (projectToUpdate == null)
                return false;

            try
            {
                projectToUpdate = updateProject;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
        public async Task<bool> DeleteProject(int id)
        {
            var projectToDelete = await GetProject(id);

            if (projectToDelete == null)
                return false;

            try
            {
                _context.Project.Remove(projectToDelete);
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