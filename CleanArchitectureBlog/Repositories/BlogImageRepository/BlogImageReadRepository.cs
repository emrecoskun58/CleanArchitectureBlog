using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.BlogImageRepository
{
    public class BlogImageReadRepository : ReadRepository<BlogImage>, IBlogImageReadRepository
    {
        public BlogImageReadRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
