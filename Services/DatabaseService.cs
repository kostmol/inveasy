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

        #region User related services
        // Method to retrieve user with given username
        public async Task<User> GetUser(string username) => await _context.User.FirstOrDefaultAsync(d => d.Username == username);               
        
        // Method to add a new user
        public async Task<bool> AddUser(User user)
        {
            if (GetUser(user.Username) != null)                
                return false; // User already exists                                
            
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
                if (desiredRole == null) 
                    return false; // Role with given id doesn't exist

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

        // Method that removes role with given role id from user with given username
        public async Task<bool> RemoveRoleFromUser(int roleId, string username)
        {
            // Get user
            User user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return false;

            // If user already does not have given role, return true
            if (!user.Roles.Select(d => d.Id == roleId).Any())
                return true;

            try
            {
                Role desiredRole = await _context.Role.FirstOrDefaultAsync(d => d.Id == roleId);
                if (desiredRole == null) return false; // Role with given id doesn't exist

                user.Roles.Remove(desiredRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Method that removes role with given role name to user with given username
        public async Task<bool> RemoveRoleFromUser(string roleName, string username)
        {
            // Get user
            User user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return false;

            // If user already has given role, return true
            if (!user.Roles.Select(d => d.Name == roleName).Any())
                return true;

            try
            {
                Role desiredRole = await _context.Role.FirstOrDefaultAsync(d => d.Name == roleName);
                if (desiredRole == null) return false; // Role with given name doesn't exist

                user.Roles.Remove(desiredRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Method that gets donations made by a user with given username
        public async Task<List<Donation>> GetUserDonations(string username)
        {
            // Get user
            var user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return null;

            try
            {
                return await _context.Donation.Where(d => d.User.Username == username).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<View>> GetUserViews(string username)
        {
            // Get user
            var user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return null;

            try
            {
                return await _context.View.Where(d => d.User.Username == username).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Project>> GetUserProjects(string username)
        {
            // Get user
            var user = await GetUser(username);

            // If user doesn't exist, do nothing
            if (user == null)
                return null;

            // If user doesn't have role of Project creator, do nothing
            if (!user.Roles.Select(d => d.Id == 1).Any())
                return null;

            try
            {
                return await _context.Project.Where(d => d.User.Username == username).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Project related services
        // Method that returns list of all projects
        public async Task<List<Project>> GetProjects() => await _context.Project.ToListAsync();

        // Method that returns list of project with given project name
        public async Task<List<Project>> GetProjectsByName(string name) => await _context.Project.Where(p => p.Name == name).ToListAsync();

        // Method that returns a project with the given project id
        public async Task<Project> GetProjectById(int id) => await _context.Project.FirstOrDefaultAsync(p => p.Id == id);        

        // Method that adds project to db
        public async Task<bool> AddProject(Project project)
        {
            if (GetProjectById(project.Id) != null)
                return false; // Project already exists           

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

        // Method that updates project with given id
        public async Task<bool> UpdateProject(int projectId, Project updatedProject)
        {
            // Get project
            Project projectToUpdate = await GetProjectById(projectId);
            
            if (projectToUpdate == null)
                return false; // Project doesn't exist

            try
            {
                // Update project and save changes
                projectToUpdate = updatedProject;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        
        #endregion
    }





}