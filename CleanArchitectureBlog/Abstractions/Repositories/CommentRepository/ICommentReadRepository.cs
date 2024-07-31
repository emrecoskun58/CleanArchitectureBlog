using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;

namespace CleanArchitectureBlog.Abstractions.Repositories.CommentRepository
{
    public interface ICommentReadRepository : IReadRepository<Comment>
    {
        Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(string Id);
        Task<bool> HasUserCommentedOnBlogAsync(Guid blogId, string userId);

    }
}
