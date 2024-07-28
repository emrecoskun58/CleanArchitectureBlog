using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.CommentRepository
{
    public class CommentWriteRepository : WriteRepository<Comment>, ICommentWriteRepository
    {
        public CommentWriteRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
