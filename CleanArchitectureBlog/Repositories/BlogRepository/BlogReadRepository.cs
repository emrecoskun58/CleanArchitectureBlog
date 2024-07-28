using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.BlogRepository
{
    public class BlogReadRepository : ReadRepository<Blog>, IBlogReadRepository
    {
        public BlogReadRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
