using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;

namespace CleanArchitectureBlog.Repositories.BlogImageRepository
{
    public class BlogImageWriteRepository : WriteRepository<BlogImage>, IBlogImageWriteRepository
    {
        public BlogImageWriteRepository(CleanArchitectureDbContext context) : base(context)
        {
        }
    }
}
