using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.BlogRepository
{
    public class BlogWriteRepository : WriteRepository<Blog>, IBlogWriteRepository
    {
        public BlogWriteRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
