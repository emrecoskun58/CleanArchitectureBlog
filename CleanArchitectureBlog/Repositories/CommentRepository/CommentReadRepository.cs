using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureBlog.Repositories.CommentRepository
{
    public class CommentReadRepository : ReadRepository<Comment>, ICommentReadRepository
    {
        private readonly CleanArchitectureDbContext _context;
        public CommentReadRepository(CleanArchitectureDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CommentViewModel>> GetCommentsByBlogIdAsync(string Id)
        {
            var comments = await _context.Comments.Where(c => c.BlogId.ToString() == Id).Select(c => new CommentViewModel
            {
                Id = c.Id,
                UserId = c.UserId,
                UserName = c.User.UserName,
                BlogId = c.BlogId.ToString(),
                Content = c.Content
            }).ToListAsync();
            return comments;
        }

        public async Task<bool> HasUserCommentedOnBlogAsync(Guid blogId, string userId)
        {
            return await _context.Comments
                .AnyAsync(c => c.BlogId == blogId && c.UserId == userId);
        }
    }
}
