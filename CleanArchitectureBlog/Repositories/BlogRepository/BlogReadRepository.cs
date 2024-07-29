using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureBlog.Repositories.BlogRepository
{
    public class BlogReadRepository : ReadRepository<Blog>, IBlogReadRepository
    {
        private readonly CleanArchitectureDbContext _context;
        public BlogReadRepository(CleanArchitectureDbContext context) : base(context)
        {
           _context = context;
        }

        public async Task<PaginatedBlogViewModel> GetBlogsAsync(int pageNumber, int pageSize)
        {
            var blogs = await _context.Blogs
            .Include(b => b.User)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(b => new BlogViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                CreatedAt = b.CreatedAt,
                Order = b.Order,
                UserId = b.UserId,
                UserName = b.User.UserName,
                BlogImages = b.BlogImages.Select(img => new BlogImageViewModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl.ToString()
                }).ToList(),
                Comments = b.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    CreatedAt = c.CreatedAt
                }).ToList(),
                Likes = b.Likes.Select(l => new LikeViewModel
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    UserName = l.User.UserName
                }).ToList()
            }).ToListAsync();

            var totalBlogs = await _context.Blogs.CountAsync();

            return new PaginatedBlogViewModel
            {
                Blogs = blogs,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalBlogs / (double)pageSize)
            };
        }
    }
}
