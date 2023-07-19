using Inveasy.Data;
using Inveasy.Models;
using Inveasy.Services;
using Inveasy.Services.ProjectServices;
using Inveasy.Services.UserServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using static Inveasy.Services.ServiceStatus;

namespace Inveasy.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly InveasyContext _context;
        private readonly CommentStatus _status;

        private readonly IUserService _userService;
        private readonly IProjectService _projectService;

        public string StatusMessage { get; private set; } = "CommentService initialized";

        public CommentService(InveasyContext context, CommentStatus serviceStatus, IUserService userService, IProjectService projectService)
        {
            _context = context;
            _status = serviceStatus;
            _userService = userService;
            _projectService = projectService;
        }

        #region GET services
        public async Task<Comment> GetCommentAsync(int id)
        {
            var comments = await GetCommentsAsync();
            return comments.FirstOrDefault(p => p.Id == id);
        }

        // Method that returns comments made by user from given username
        public async Task<List<Comment>> GetUserCommentsAsync(string username) => await GetUserCommentsAsync(await _userService.GetUserAsync(username));

        // Method that returns comments made by user from given user id
        public async Task<List<Comment>> GetUserCommentsAsync(int userId) => await GetUserCommentsAsync(await _userService.GetUserAsync(userId));

        // Method that returns comments made by user from given user 
        public async Task<List<Comment>> GetUserCommentsAsync(User user)
        {
            // If user doesn't exist, do nothing
            if (user == null)
            {
                StatusMessage = _status.ErrorGetStatus("Null user");
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
                StatusMessage = _status.ExceptionGetStatus(ex.Message);
                return null;
            }
        }
        #endregion

        #region ADD services
        public async Task<bool> AddCommentAsync(int projectId, User user, string commentText)
            => await AddCommentAsync(await _projectService.GetProjectAsync(projectId), user, commentText);

        public async Task<bool> AddCommentAsync(Project project, int userId, string commentText)
            => await AddCommentAsync(project, await _userService.GetUserAsync(userId), commentText);

        public async Task<bool> AddCommentAsync(Project project, string username, string commentText)
            => await AddCommentAsync(project, await _userService.GetUserAsync(username), commentText);

        public async Task<bool> AddCommentAsync(int projectId, string username, string commentText)
            => await AddCommentAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(username), commentText);

        public async Task<bool> AddCommentAsync(int projectId, int userId, string commentText)
            => await AddCommentAsync(await _projectService.GetProjectAsync(projectId), await _userService.GetUserAsync(userId), commentText);

        public async Task<bool> AddCommentAsync(Project project, User user, string commentText)
        {
            if (project == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null user");
                return false;
            }

            if (user == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null project");
                return false;
            }

            // Create comment
            Comment comment = new Comment
            {
                Text = commentText,
                DateTime = DateTime.Now,
                User = user,
                Project = project
            };

            var commentAdded = await AddCommentAsync(comment);

            if (!commentAdded)
                return false;

            return true;
        }

        public async Task<bool> AddCommentAsync(Comment comment)
        {
            if (comment == null)
            {
                StatusMessage = _status.ErrorAddStatus("Null comment");
                return false;
            }

            // Don't allow empty comments and comments over 500 characters 
            if (comment.Text.Length > 500 || comment.Text.IsNullOrEmpty())
            {
                StatusMessage = _status.ErrorAddStatus("Comment text is empty or is over 500 characters");
                return false;
            }

            try
            {
                // Check if comment exists
                if (_context.Comment.Any(d => d.Id == comment.Id))
                {
                    StatusMessage = _status.WarningGetStatus("Comment already exists in database");
                    return true;
                }

                // Store comment
                _context.Comment.AddAsync(comment);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessAddStatus(comment.Text);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionAddStatus(ex.Message, comment.Text);
                return false;
            }
        }
        #endregion

        #region Comment UPDATE/DELETE services
        // Method that updates comment from given comment and given comment text
        public async Task<bool> UpdateComment(Comment commentToUpdate, string commentText) => await UpdateComment(commentToUpdate, new Comment { Text = commentText });

        // Method that updates comment from given comment id and given comment text
        public async Task<bool> UpdateComment(int commentId, string commentText) => await UpdateComment(await GetCommentAsync(commentId), new Comment { Text = commentText });

        // Method that updates comment from given comment id and given comment
        public async Task<bool> UpdateComment(int commentId, Comment updatedComment) => await UpdateComment(await GetCommentAsync(commentId), updatedComment);

        // Method that updates given comment
        public async Task<bool> UpdateComment(Comment commentToUpdate, Comment updatedComment)
        {
            if (commentToUpdate == null)
            {
                StatusMessage = _status.ErrorUpdateStatus("Null comment");
                return false;
            }

            if (updatedComment.Text.Length > 500 || commentToUpdate.Text.IsNullOrEmpty())
            {
                StatusMessage = _status.ErrorAddStatus("Comment text is empty or is over 500 characters");
                return false;
            }

            try
            {
                // Check if comment already exists
                if (!_context.Comment.Any(d => d.Id == commentToUpdate.Id))
                {
                    StatusMessage = _status.WarningGetStatus("Comment doesn't exist in database");
                    return false;
                }

                // Update project and save changes
                if (updatedComment.Text != updatedComment.Text) commentToUpdate.Text = updatedComment.Text;
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessUpdateStatus(updatedComment.Text);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionUpdateStatus(ex.Message, commentToUpdate.Text);
                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(int commentId) => await DeleteCommentAsync(await GetCommentAsync(commentId));

        public async Task<bool> DeleteCommentAsync(Comment commentToDelete)
        {
            if (commentToDelete == null)
            {
                StatusMessage = _status.WarningGetStatus("Null comment");
                return true;
            }

            try
            {
                // Check if comment already exists
                if (!_context.Comment.Any(d => d.Id == commentToDelete.Id))
                {
                    StatusMessage = _status.WarningGetStatus("Comment doesn't exist in database");
                    return false;
                }

                // Delete comment from database
                _context.Comment.Remove(commentToDelete);
                await _context.SaveChangesAsync();
                StatusMessage = _status.SuccessDeleteStatus(commentToDelete.Text);
                return true;
            }
            catch (Exception ex)
            {
                StatusMessage = _status.ExceptionDeleteStatus(ex.Message, commentToDelete.Text);
                return false;
            }
        }
        #endregion
    }
}
