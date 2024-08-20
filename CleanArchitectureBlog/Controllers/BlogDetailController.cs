using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureBlog.Controllers
{
    public class BlogDetailController : Controller
    {
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILikeReadRepository _likeReadRepository;
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IBlogImageReadRepository _blogImageReadRepository;

        public BlogDetailController(IBlogReadRepository blogReadRepository, UserManager<ApplicationUser> userManager, ILikeReadRepository likeReadRepository, ICommentReadRepository commentReadRepository, IBlogImageReadRepository blogImageReadRepository)
        {
            _blogReadRepository = blogReadRepository;
            _userManager = userManager;
            _likeReadRepository = likeReadRepository;
            _commentReadRepository = commentReadRepository;
            _blogImageReadRepository = blogImageReadRepository;
        }

        [HttpGet("blogDetail/{slug}")]
        public async Task<IActionResult> BlogDetail(string slug)
        {
            var blog = await _blogReadRepository.GetBlogBySlugAsync(slug);
            var user = await _userManager.FindByIdAsync(blog.UserId);

            if (blog == null)
            {
                return NotFound();
            }

            BlogViewModel blogViewModel = new BlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                UserId = blog.UserId,
                UserName = user.UserName,
                Likes = await _likeReadRepository.GetLikesByBlogIdAsync(blog.Id.ToString()),
                Comments = await _commentReadRepository.GetCommentsByBlogIdAsync(blog.Id.ToString()),
                BlogImage = await _blogImageReadRepository.GetBlogImageByBlogAsync(blog.Id.ToString())
            };

            return View(blogViewModel);
        }
    }
}
