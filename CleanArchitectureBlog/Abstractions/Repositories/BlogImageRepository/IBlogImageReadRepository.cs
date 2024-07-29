using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;

namespace CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository
{
    public interface IBlogImageReadRepository : IReadRepository<BlogImage>
    {
        Task<BlogImageViewModel> GetBlogImageByBlogAsync(string Id);
    }
}
