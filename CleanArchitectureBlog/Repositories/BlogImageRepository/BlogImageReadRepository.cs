using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Contexts;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureBlog.Repositories.BlogImageRepository
{
    public class BlogImageReadRepository : ReadRepository<BlogImage>, IBlogImageReadRepository
    {
        private readonly CleanArchitectureDbContext _context;
        public BlogImageReadRepository(CleanArchitectureDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BlogImageViewModel> GetBlogImageByBlogAsync(string Id)
        {
            var image = await _context.BlogImages.FirstOrDefaultAsync(x => x.BlogId.ToString() == Id);
            var imageViewModel = new BlogImageViewModel
            {
                Id = image.Id,
                ImageUrl = image.ImageUrl
            };

            return imageViewModel;
        }
    }
}
