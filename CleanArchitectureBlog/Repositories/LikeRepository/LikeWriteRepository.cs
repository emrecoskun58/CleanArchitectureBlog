using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.LikeRepository
{
    public class LikeWriteRepository : WriteRepository<Like>, ILikeWriteRepository
    {
        public LikeWriteRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
