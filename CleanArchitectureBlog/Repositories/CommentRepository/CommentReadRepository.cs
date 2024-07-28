using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.CommentRepository
{
    public class CommentReadRepository : ReadRepository<Comment>, ICommentReadRepository
    {
        public CommentReadRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
