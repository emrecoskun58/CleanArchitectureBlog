using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureBlog.Repositories.LikeRepository
{
    public class LikeReadRepository : ReadRepository<Like>, ILikeReadRepository
    {
        private readonly CleanArchitectureDbContext _context;
        public LikeReadRepository(CleanArchitectureDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<LikeViewModel>> GetLikesByBlogIdAsync(string Id)
        {
            var likes = await _context.Likes.Where(l => l.BlogId.ToString() == Id).Select(l => new LikeViewModel
            {
                Id = l.Id,
                UserId = l.UserId,
                UserName = l.User.UserName,
                BlogId = l.BlogId.ToString()
            }).ToListAsync();
            return likes;
        }
    }
}
