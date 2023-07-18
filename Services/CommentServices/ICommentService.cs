using Inveasy.Models;

namespace Inveasy.Services.CommentServices
{
    public interface ICommentService
    {
        string StatusMessage { get; }

        Task<Comment> GetCommentAsync(int id);
        Task<List<Comment>> GetUserCommentsAsync(string username);
        Task<List<Comment>> GetUserCommentsAsync(int userId);
        Task<List<Comment>> GetUserCommentsAsync(User user);
        Task<List<Comment>> GetCommentsAsync();
        Task<bool> AddCommentAsync(int projectId, User user, string commentText);
        Task<bool> AddCommentAsync(Project project, int userId, string commentText);
        Task<bool> AddCommentAsync(Project project, string username, string commentText);
        Task<bool> AddCommentAsync(int projectId, string username, string commentText);
        Task<bool> AddCommentAsync(int projectId, int userId, string commentText);
        Task<bool> AddCommentAsync(Project project, User user, string commentText);
        Task<bool> AddCommentAsync(Comment comment);
        Task<bool> UpdateComment(Comment commentToUpdate, string commentText);
        Task<bool> UpdateComment(int commentId, string commentText);
        Task<bool> UpdateComment(int commentId, Comment updatedComment);
        Task<bool> UpdateComment(Comment commentToUpdate, Comment updatedComment);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<bool> DeleteCommentAsync(Comment commentToDelete);
    }
}
