using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Abstractions.Services;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleanArchitectureBlog.Controllers
{
    public class PanelController : Controller
    {
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogImageWriteRepository _blogImageWriteRepository;
        private readonly IBlogImageReadRepository _blogImageReadRepository;
        private readonly ILikeReadRepository _likeReadRepository;
        private readonly ILikeWriteRepository _likeWriteRepository;
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PanelController(IBlogWriteRepository blogWriteRepository, IBlogImageWriteRepository blogImageWriteRepository, IImageService imageService, UserManager<ApplicationUser> userManager, IBlogReadRepository blogReadRepository, IBlogImageReadRepository blogImageReadRepository, ILikeReadRepository likeReadRepository, ICommentReadRepository commentReadRepository, ICommentWriteRepository commentWriteRepository, ILikeWriteRepository likeWriteRepository)
        {
            _blogWriteRepository = blogWriteRepository;
            _blogImageWriteRepository = blogImageWriteRepository;
            _imageService = imageService;
            _userManager = userManager;
            _blogReadRepository = blogReadRepository;
            _blogImageReadRepository = blogImageReadRepository;
            _likeReadRepository = likeReadRepository;
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _likeWriteRepository = likeWriteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserBlogList()
        {
            var userName = Request.Cookies["UserName"];
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Auth");
            }
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            IEnumerable<BlogViewModel> blogs = await _blogReadRepository.GetBlogsByUserIdAsync(user.Id);
            return View(blogs);
        }
        [HttpGet]
        public IActionResult AddBlog()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBlog(CreateBlogViewModel blogViewModel)
        {

            if (ModelState.IsValid)
            {
                var userName = Request.Cookies["UserName"];
                if (string.IsNullOrEmpty(userName))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return RedirectToAction("Login", "Auth");
                }
                blogViewModel.UserId = user.Id;

                var blog = new Blog
                {
                    Id = Guid.NewGuid(),
                    Title = blogViewModel.Title,
                    Content = blogViewModel.Content,
                    Order = blogViewModel.Order,
                    UserId = user.Id, 
                    User = user,
                    
                };

                await _blogWriteRepository.AddAsync(blog);
                await _blogWriteRepository.SaveAsync();

                if (blogViewModel.BlogImages != null)
                {
                    string filePath = await _imageService.SaveImageAsync(blogViewModel.BlogImages, blog.Id.ToString());

                    var blogImage = new BlogImage
                    {
                        BlogId = blog.Id,
                        ImageUrl = filePath
                    };
                    await _blogImageWriteRepository.AddAsync(blogImage);
                    await _blogImageWriteRepository.SaveAsync();
                }

                return RedirectToAction("Index");
            }
            return View(blogViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBlog(string id)
        {
            var blog = await _blogReadRepository.GetByIdAsync(id);
            if (blog == null)           
                return NotFound();
            var likes = await _likeReadRepository.GetLikesByBlogIdAsync(id);
            var comments = await _commentReadRepository.GetCommentsByBlogIdAsync(id);
            var blogImage = await _blogImageReadRepository.GetBlogImageByBlogAsync(id);
            await _blogImageWriteRepository.RemoveAsync(blogImage.Id.ToString());
            await _blogWriteRepository.RemoveAsync(blog.Id.ToString());
            foreach (var like in likes)
            {
                await _likeWriteRepository.RemoveAsync(like.Id.ToString());
            }
            foreach (var comment in comments)
            {
                await _commentWriteRepository.RemoveAsync(comment.Id.ToString());
            }
            await _blogWriteRepository.SaveAsync();

            return RedirectToAction("UserBlogList", "Panel");
        }

    }
}
