using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.Build.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            .Where(b => b.IsActive)
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
                Slug = b.Slug,
                BlogImage = new BlogImageViewModel
                {
                    Id = b.BlogImage.Id,
                    ImageUrl = b.BlogImage.ImageUrl
                },

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

        public async Task<IEnumerable<BlogViewModel>> GetBlogsByUserIdAsync(string Id)
        {
            return await _context.Blogs
                .Where(b => b.UserId == Id)
                .Where(b => b.IsActive)
                .Include(b => b.User)
                .Include(b => b.BlogImage)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    CreatedAt = b.CreatedAt,
                    Order = b.Order,
                    UserId = b.UserId,
                    UserName = b.User.UserName,
                    Slug = b.Slug,
                    BlogImage = new BlogImageViewModel
                    {
                        Id = b.BlogImage.Id,
                        ImageUrl = b.BlogImage.ImageUrl
                    },
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
        }

        public async Task<BlogViewModel> GetBlogBySlugAsync(string slug)
        {
            return await _context.Blogs
                .Where(b => b.Slug == slug && b.IsActive)
                .Include(b => b.User)
                .Include(b => b.BlogImage)
                .Include(b => b.Comments)
                    .ThenInclude(c => c.User)
                .Include(b => b.Likes)
                    .ThenInclude(l => l.User)
                .Select(b => new BlogViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Slug = b.Slug,
                    Content = b.Content,
                    CreatedAt = b.CreatedAt,
                    Order = b.Order,
                    UserId = b.UserId,
                    UserName = b.User.UserName,
                    BlogImage = b.BlogImage != null ? new BlogImageViewModel
                    {
                        Id = b.BlogImage.Id,
                        ImageUrl = b.BlogImage.ImageUrl
                    } : null,
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
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> GetBlogsForTitle(string title)
        {
            var result = _context.Blogs.Where(x => x.Title == title);
            if (!result.IsNullOrEmpty())
                return true;
            return false;
        }
    }
}
