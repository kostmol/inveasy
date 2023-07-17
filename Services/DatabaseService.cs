using Humanizer.Localisation.TimeToClockNotation;
using Inveasy.Data;
using Inveasy.Models;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Project = Inveasy.Models.Project;
using View = Inveasy.Models.View;

namespace Inveasy.Services
{
    public class DatabaseService
    {
        private readonly InveasyContext _context;
        public string statusMessage = "DatabaseService initialized"; // ERROR: Internal error raised, WARNING: Something unexpected happened, EXCEPTION RAISED: an exception was raised

        public DatabaseService(InveasyContext context)
        {
            _context = context;
        }

        #region GET services

        #region User GET services
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
                ?.Include(t => t.Donations).ThenInclude(o => o.Project)
                .ToListAsync();
            }
            catch(Exception ex) 
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch users: {ex.Message}";
                return null;
            }
        }                    
        #endregion

        #region Project GET services               
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
        public async Task<List<Project>> GetUserProjectsAsync(string username) => await GetUserProjectsAsync(await GetUserAsync(username));
   
        // Method that returns all projects started by user from given user id
        public async Task<List<Project>> GetUserProjectsAsync(int userId) => await GetUserProjectsAsync(await GetUserAsync(userId));

        // Method that returns all projects started by user from given user 
        public async Task<List<Project>> GetUserProjectsAsync(User user)
        {
            // If user doesn't have role of Project creator, do nothing
            if (!user.Roles.Any(d => d.Id == 3))
                return null;

            try
            {
                return await _context.Project.Where(d => d.User.Id == user.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch projects: {ex.Message}";
                return null;
            }
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
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Donation GET services
        // Method that gets donations with given donation id
        public async Task<Donation> GetDonationAsync(int donationId)
        {
            var donations = await GetDonationsAsync();
            return donations?.FirstOrDefault(d => d.Id == donationId);
        } 

        // Method that gets donations made by a user with given username
        public async Task<List<Donation>> GetUserDonationsAsync(string username) => await GetUserDonationsAsync(await GetUserAsync(username));                    

        // Method that gets donations made by a user with given user id
        public async Task<List<Donation>> GetUserDonationsAsync(int userId) => await GetUserDonationsAsync(await GetUserAsync(userId));
        

        // Method that gets donations made by a user with given user
        public async Task<List<Donation>> GetUserDonationsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                statusMessage = $"WARNING - Unable to fetch donations: Null user";
                return null;
            }
                
            var donations = await GetDonationsAsync();
            return donations?.Where(d => d.User.Id == user.Id).ToList();
        }

        public async Task<List<Donation>> GetDonationsAsync()
        {
            try
            {
                return await _context.Donation
                    ?.Include(p => p.Project)
                    ?.Include(s => s.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch donations: {ex.Message}";
                return null;
            }
        }
        #endregion

        #region View GET services
        // Method that gets views with given view id
        public async Task<View> GetViewAsync(int viewId)
        {
            var views = await GetViewsAsync();
            return views?.FirstOrDefault(d => d.Id == viewId);
        } 

        // Method that returns views made by user from given username
        public async Task<List<View>> GetUserViewsAsync(string username) => await GetUserViewsAsync(await GetUserAsync(username));

        // Method that returns views made by user from given user id
        public async Task<List<View>> GetUserViewsAsync(int userId) => await GetUserViewsAsync(await GetUserAsync(userId));

        // Method that returns views made by user from given user 
        public async Task<List<View>> GetUserViewsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                statusMessage = $"WARNING - Unable to fetch views: Null user";
                return null;
            }
                
            var views = await GetViewsAsync();
            return views?.Where(d => d.User.Id == user.Id).ToList();
        }

        public async Task<List<View>> GetViewsAsync()
        {
            try
            {
                return await _context.View
                    ?.Include(p => p.Project)
                    ?.Include(s => s.User)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch views: {ex.Message}";
                return null;
            }
        }
        #endregion        

        #region Comment GET services
        public async Task<Comment> GetCommentAsync(int id)
        {
            var comments = await GetCommentsAsync();
            return comments.FirstOrDefault(p => p.Id == id);
        }

        // Method that returns comments made by user from given username
        public async Task<List<Comment>> GetUserCommentsAsync(string username) => await GetUserCommentsAsync(await GetUserAsync(username));

        // Method that returns comments made by user from given user id
        public async Task<List<Comment>> GetUserCommentsAsync(int userId) => await GetUserCommentsAsync(await GetUserAsync(userId));

        // Method that returns comments made by user from given user 
        public async Task<List<Comment>> GetUserCommentsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                statusMessage = $"WARNING - Unable to fetch comments: Null user";
                return null;
            }                

            var comments = await GetCommentsAsync();
            return comments?.Where(d => d.User.Id == user.Id).ToList();
        }

        // Method that returns all commments
        public async Task<List<Comment>> GetCommentsAsync()
        {
            try
            {
                return await _context.Comment
                    ?.Include(p => p.Project)
                    ?.Include(s => s.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch comments: {ex.Message}";
                return null;
            }
        }

        #endregion

        #region Role GET services
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
                statusMessage = $"EXCEPTION RAISED - Unable to fetch roles: {ex.Message}";
                return null;
            }
        }
        #endregion

        #region Category GET services
        // Method that return role from given role id
        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            var categories = await GetCategoriesAsync();
            return categories?.FirstOrDefault(d => d.Id == categoryId);
        }

        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            var categories = await GetCategoriesAsync();
            return categories?.FirstOrDefault(d => d.Name == categoryName);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Category.ToListAsync();
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch categories: {ex.Message}";
                return null;
            }
        }
        #endregion

        #endregion

        #region ADD services

        #region User ADD services
        // Method to add a new user
        public async Task<bool> AddUserAsync(User user)
        {
            if (user == null)
            {
                statusMessage = $"ERROR - Null user can't be added";
                return false; // User already exists                                
            }

            User userToAdd = await GetUserAsync(user.Username);

            if (userToAdd != null)
            {
                statusMessage = $"ERROR - User {userToAdd.Username} with id {userToAdd.Id} could not be added: User already exists";
                return false; // User already exists                                
            }                

            try
            {
                // Adding given user
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                statusMessage = $"SUCCESS - User {user.Name} succesfully added to database";
                return true;
            }
            catch (Exception ex)
            {
                statusMessage += $"EXCEPTION RAISED - User {user.Username} with id {user.Id} could not be added: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region Project ADD services
        // Method that adds project to db
        public async Task<bool> AddProjectAsync(Project project)
        {
            try
            {
                await _context.Project.AddAsync(project);
                await _context.SaveChangesAsync();
                statusMessage = $"SUCCESS - Project {project.Name} succesfully added to database";
                return true;
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to fetch roles: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region Donation ADD services
        public async Task<bool> AddDonationAsync(int projectId , string username, double donationAmount) => await AddDonationAsync(await GetProjectAsync(projectId), await GetUserAsync(username), donationAmount);

        public async Task<bool> AddDonationAsync(Project project, string username, double donationAmount) => await AddDonationAsync(project, await GetUserAsync(username), donationAmount);
        
        public async Task<bool> AddDonationAsync(Project project, int userId, double donationAmount) => await AddDonationAsync(project, await GetUserAsync(userId), donationAmount);

        public async Task<bool> AddDonationAsync(int projectId, int userId, double donationAmount) => await AddDonationAsync(await GetProjectAsync(projectId), await GetUserAsync(userId), donationAmount);        

        public async Task<bool> AddDonationAsync(Project project, User user, double donationAmount)
        {            
            if (project == null)
            {
                statusMessage = $"WARNING - Unable to add donation: Null project";
                return false; 
            }                

            if (user == null)
            {
                statusMessage = $"WARNING - Unable to add donation: Null user";
                return false;
            }                

            // Create Donation
            Donation donation = new Donation
            {
                Amount = Math.Round(donationAmount, 2),
                Date = DateTime.Now,
                User = user,
                Project = project
            };

            var donationAdded = await AddDonationAsync(donation);

            if (!donationAdded) 
                return false;

            // Add donation amount to project
            project.FundAmount += donationAmount;

            statusMessage = $"SUCCESS - Donation of {donation.Amount} by user {user.Username} given to project {project.Name} succesfully added to database";
            return true;
        }

        public async Task<bool> AddDonationAsync(Donation donation)
        {
            // Don't allow empty or negative donations
            if (donation.Amount <= 0)
            {
                statusMessage = $"ERROR - Unable to add donation: Donation amount is less or equal to zero";
                return false;
            }

            try
            {
                // Store Donation
                _context.Donation.AddAsync(donation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to add donation: {ex.Message}";
                return false;
            }
        }
        #endregion

        #region View ADD services
        public async Task<bool> AddViewAsync(Project project, int userId) => await AddViewAsync(project, await GetUserAsync(userId));

        public async Task<bool> AddViewAsync(Project project, string username) => await AddViewAsync(project, await GetUserAsync(username));

        public async Task<bool> AddViewAsync(int projectId, User user) => await AddViewAsync(await GetProjectAsync(projectId), user);

        public async Task<bool> AddViewAsync(int projectId, string username) => await AddViewAsync(await GetProjectAsync(projectId), await GetUserAsync(username));

        public async Task<bool> AddViewAsync(int projectId, int userId) => await AddViewAsync(await GetProjectAsync(projectId), await GetUserAsync(userId));

        public async Task<bool> AddViewAsync(Project project, User user)
        {
            if (project == null)
            {
                statusMessage = $"WARNING - Unable to add view: Null project";
                return false;
            }                

            if (user == null)
            {
                statusMessage = $"WARNING - Unable to add view: Null user";
                return false;
            }                

            // Create View
            View view = new View
            {
                Date = DateTime.Now,
                User = user,
                Project = project
            };

            var viewAdded = await AddViewAsync(view);

            if(!viewAdded)
                return false;

            statusMessage = $"SUCCESS - View of {project.Name} by user {user.Username} succesfully added to database";
            return true;
        }

        public async Task<bool> AddViewAsync(View view)
        {
            try 
            { 
                // Store View
                _context.View.AddAsync(view);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to add view: {ex.Message}";
                return false;
            }
        }

        #endregion

        #region Comment ADD services
        public async Task<bool> AddCommentAsync(int projectId, User user, string commentText)
            => await AddCommentAsync(await GetProjectAsync(projectId), user, commentText);

        public async Task<bool> AddCommentAsync(Project project, int userId, string commentText)
            => await AddCommentAsync(project, await GetUserAsync(userId), commentText);

        public async Task<bool> AddCommentAsync(Project project , string username, string commentText)
            => await AddCommentAsync(project, await GetUserAsync(username), commentText);

        public async Task<bool> AddCommentAsync(int projectId, string username, string commentText)
            => await AddCommentAsync(await GetProjectAsync(projectId), await GetUserAsync(username), commentText);

        public async Task<bool> AddCommentAsync(int projectId, int userId, string commentText) 
            => await AddCommentAsync(await GetProjectAsync(projectId), await GetUserAsync(userId), commentText);
            
        public async Task<bool> AddCommentAsync(Project project, User user, string commentText)
        {
            if (project == null)
            {
                statusMessage = $"WARNING - Unable to add comment: Null user";
                return false;
            }                

            if (user == null)
            {
                statusMessage = $"WARNING - Unable to add comment: Null project";
                return false;
            }                

            // Create comment
            Comment comment = new Comment
            {
                Text = commentText,
                dateTime = DateTime.Now,
                User = user,
                Project = project
            };

            var commentAdded = await AddCommentAsync(comment);

            if(!commentAdded)
                return false;

            statusMessage = $"SUCCESS - Comment of {project.Name} by user {user.Username} succesfully added to database";
            return true;
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {            
            try
            {
                if(comment == null)
                {
                    statusMessage = $"ERROR - Unable to add comment: Null comment";
                    return false;
                }                    
                
                // Don't allow empty comments and comments over 500 characters 
                if (comment.Text.Length > 500 || comment.Text.IsNullOrEmpty())
                {
                    statusMessage = $"ERROR - Unable to add comment: Comment text is empty or over 500 characters";
                    return false;
                }
                    
                // Store comment
                _context.Comment.AddAsync(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to add comment: {ex.Message}";
                return false;
            }
        }
        #endregion

        #endregion

        #region UPDATE/DELETE services

        #region User UPDATE/DELETE services
        // Method to update a user with a given username
        public async Task<bool> UpdateUserAsync(string username, User updatedUser) => await UpdateUserAsync(await GetUserAsync(username), updatedUser);

        // Method to update a user with a given username
        public async Task<bool> UpdateUserAsync(int userId, User updatedUser) => await UpdateUserAsync(await GetUserAsync(userId), updatedUser);

        public async Task<bool> UpdateUserAsync(User userToUpdate, User updatedUser)
        {
            // If user doesn't exist, do nothing
            if (userToUpdate == null || updatedUser == null)
            {
                statusMessage = $"WARNING - Unable to update user: Null user";
                return false;
            }
                
            try
            {
                List<string> updatedFields = new List<string>();

                // Update user and save changes
                if (updatedUser.Username != null) 
                {
                    userToUpdate.Username = updatedUser.Username;
                    updatedFields.Add("username");
                }                
                if(updatedUser.Password != null) userToUpdate.Password = updatedUser.Password;
                if (updatedUser.Email != null) userToUpdate.Email = updatedUser.Email;
                if (updatedUser.Name != null) userToUpdate.Name = updatedUser.Name;
                if (updatedUser.Surname != null) userToUpdate.Surname = updatedUser.Surname;
                if (updatedUser.Birthday != default(DateTime)) userToUpdate.Birthday = updatedUser.Birthday;
                if (updatedUser.Roles != null) userToUpdate.Roles = updatedUser.Roles;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                statusMessage = $"EXCEPTION RAISED - Unable to update user: {ex.Message}";
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

        /*
        // Method that adds role with given role id to user with given username
        public async Task<bool> AddRoleToUserAsync(int roleId, string username) => await AddRoleToUserAsync(roleId, await GetUserAsync(username));

        // Method that adds role with given role name to user with given username
        public async Task<bool> AddRoleToUserAsync(string roleName, string username) => await AddRoleToUserAsync(roleName, await GetUserAsync(username));

        // Method that adds role with given role id to user from given user id
        public async Task<bool> AddRoleToUserAsync(int roleId, int userId) => await AddRoleToUserAsync(roleId, await GetUserAsync(userId));

        // Method that adds role with given role name to user from given user id
        public async Task<bool> AddRoleToUserAsync(string roleName, int userId) => await AddRoleToUserAsync(roleName, await GetUserAsync(userId));

        // Method that adds role with given role id to given user 
        public async Task<bool> AddRoleToUserAsync(int roleId, User user) => await AddRoleToUserAsync(await GetRoleAsync(roleId), user);

        // Method that adds role with given role id to given user 
        public async Task<bool> AddRoleToUserAsync(string roleName, User user) => await AddRoleToUserAsync(await GetRoleAsync(roleName), user);

        // Method that adds given role to given user 
        public async Task<bool> AddRoleToUserAsync(Role role, User user)
        {
            if(user == null) return false;
            if (role == null) return false;

            // If the the User's Roles list is null, initialize it
            if (user.Roles is null)
                user.Roles = new List<Role>();

            try
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Method that removes role with given role id from user with given username
        public async Task<bool> RemoveRoleFromUserAsync(int roleId, string username) => await RemoveRoleFromUserAsync(await GetRoleAsync(roleId), await GetUserAsync(username));

        // Method that removes role with given role name to user with given username
        public async Task<bool> RemoveRoleFromUserAsync(string roleName, string username) => await RemoveRoleFromUserAsync(await GetRoleAsync(roleName), await GetUserAsync(username));

        // Method that removes role with given role name to user with given user id
        public async Task<bool> RemoveRoleFromUserAsync(string roleName, int userId) => await RemoveRoleFromUserAsync(await GetRoleAsync(roleName), await GetUserAsync(userId));

        // Method that removes role with given role id to user with given user id
        public async Task<bool> RemoveRoleFromUserAsync(int roleId, int userId) => await RemoveRoleFromUserAsync(await GetRoleAsync(roleId), await GetUserAsync(userId));

        // Method that removes role with given role id from user with given username
        public async Task<bool> RemoveRoleFromUserAsync(Role role, User user)
        {            
            if (user == null)
                return false;

            if(role == null ) 
                return false;

            // If user already does not have any roles, return true
            if (user.Roles is null)
                return true;
           
            try
            {
                user.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        */

        #endregion

        #region Project UPDATE/DELETE services
        // Method that updates given project
        public async Task<bool> UpdateProjectAsync(int projectToUpdateId, Project updatedProject) => await UpdateProjectAsync(await GetProjectAsync(projectToUpdateId), updatedProject);

        // Method that updates project with given id
        public async Task<bool> UpdateProjectAsync(Project projectToUpdate, Project updatedProject)
        {                         
            if (projectToUpdate == null)
                return false; // Project doesn't exist

            try
            {
                // Update project and save changes                
                if (updatedProject.Name != null) projectToUpdate.Name = updatedProject.Name;
                if (updatedProject.Description != null) projectToUpdate.Description = updatedProject.Description;
                if (updatedProject.FundAmount != 0) projectToUpdate.FundAmount= updatedProject.FundAmount;
                if (updatedProject.FundGoal != null) projectToUpdate.FundGoal = updatedProject.FundGoal;
                if (updatedProject.TrendingScore != null) projectToUpdate.TrendingScore = updatedProject.TrendingScore;
                if (updatedProject.RewardsTier != null) projectToUpdate.RewardsTier = updatedProject.RewardsTier;
                if (updatedProject.Categories != null) projectToUpdate.Categories = updatedProject.Categories;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Method that removes project with given id
        public async Task<bool> DeleteProjectAsync(int projectId) => await DeleteProjectAsync(await GetProjectAsync(projectId));
        
        // Method that removes given project 
        public async Task<bool> DeleteProjectAsync(Project projectToDelete)
        {            
            if (projectToDelete == null)
                return true; // Project doesn't exist

            try
            {
                // Update project and save changes
                _context.Project.Remove(projectToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Comment UPDATE/DELETE services
        // Method that updates comment from given comment and given comment text
        public async Task<bool> UpdateComment(Comment commentToUpdate, string commentText) => await UpdateComment(commentToUpdate, new Comment { Text = commentText });

        // Method that updates comment from given comment id and given comment text
        public async Task<bool> UpdateComment(int commentId, string commentText) => await UpdateComment(await GetCommentAsync(commentId), new Comment { Text = commentText});

        // Method that updates comment from given comment id and given comment
        public async Task<bool> UpdateComment(int commentId, Comment updatedComment) => await UpdateComment(await GetCommentAsync(commentId), updatedComment);

        // Method that updates given comment
        public async Task<bool> UpdateComment(Comment commentToUpdate, Comment updatedComment)
        {
            if (commentToUpdate == null)
                return false; // Comment doesn't exist

            if (commentToUpdate.Text.IsNullOrEmpty())
                return false; // Comment text is empty or null

            try
            {
                // Update project and save changes
                if(updatedComment.Text != null) commentToUpdate.Text = updatedComment.Text;
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId) => await DeleteCommentAsync(await GetCommentAsync(commentId));

        public async Task<bool> DeleteCommentAsync(Comment commentToDelete)
        {            
            if (commentToDelete == null)
                return true; // Comment with given id doesn't exist

            try
            {
                // Delete comment from project
                _context.Comment.Remove(commentToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #endregion
    }
}