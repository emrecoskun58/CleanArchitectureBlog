using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;

namespace CleanArchitectureBlog.Abstractions.Repositories.BlogRepository
{
    public interface IBlogReadRepository : IReadRepository<Blog>
    {
        Task<PaginatedBlogViewModel> GetBlogsAsync(int pageNumber, int pageSize);
    }
}
