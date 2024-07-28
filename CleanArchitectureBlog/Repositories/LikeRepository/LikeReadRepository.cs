using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.LikeRepository
{
    public class LikeReadRepository : ReadRepository<Like>, ILikeReadRepository
    {
        public LikeReadRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
