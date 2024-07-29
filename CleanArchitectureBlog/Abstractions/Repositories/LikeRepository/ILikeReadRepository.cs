using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;

namespace CleanArchitectureBlog.Abstractions.Repositories.LikeRepository
{
    public interface ILikeReadRepository : IReadRepository<Like>
    {
        Task<List<LikeViewModel>> GetLikesByBlogIdAsync(string Id);
    }
}
