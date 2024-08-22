using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Abstractions.Repositories.LikeRepository;
using CleanArchitectureBlog.Abstractions.Services;
using CleanArchitectureBlog.Helpers;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureBlog.Controllers
{
    public class BlogController : Controller
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

        public BlogController(IBlogWriteRepository blogWriteRepository, IBlogReadRepository blogReadRepository, IBlogImageWriteRepository blogImageWriteRepository, IBlogImageReadRepository blogImageReadRepository, ILikeReadRepository likeReadRepository, ILikeWriteRepository likeWriteRepository, ICommentReadRepository commentReadRepository, ICommentWriteRepository commentWriteRepository, IImageService imageService, UserManager<ApplicationUser> userManager)
        {
            _blogWriteRepository = blogWriteRepository;
            _blogReadRepository = blogReadRepository;
            _blogImageWriteRepository = blogImageWriteRepository;
            _blogImageReadRepository = blogImageReadRepository;
            _likeReadRepository = likeReadRepository;
            _likeWriteRepository = likeWriteRepository;
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _imageService = imageService;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> List()
        {
            IEnumerable<BlogViewModel> blogs = await _blogReadRepository.GetBlogsAsyncForAdmin();
            return View(blogs);
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public IActionResult AddBlog()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
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
                    Slug = SlugHelper.Slugify(blogViewModel.Title)

                };
                bool value = await _blogReadRepository.GetBlogsForTitle(blogViewModel.Title);
                if(value)
                    return RedirectToAction("AddBlog", "Blog");
                await _blogWriteRepository.AddAsync(blog);
                await _blogWriteRepository.SaveAsync();

                if (blogViewModel.BlogImage != null)
                {
                    string filePath = await _imageService.SaveImageAsync(blogViewModel.BlogImage, blog.Id.ToString());

                    var blogImage = new BlogImage
                    {
                        BlogId = blog.Id,
                        ImageUrl = filePath
                    };
                    await _blogImageWriteRepository.AddAsync(blogImage);
                    await _blogImageWriteRepository.SaveAsync();
                }

                return RedirectToAction("UserBlogList","Blog");
            }
            return View(blogViewModel);
        }
        [HttpGet]
        [Authorize]
        public IActionResult EditBlog(string Id)
        {
            var blog = _blogReadRepository.GetByIdAsync(Id).Result;
            if (blog == null)
                return NotFound();
            var blogImage = _blogImageReadRepository.GetBlogImageByBlogAsync(Id).Result;
            var blogViewModel = new EditBlogViewModel
            {
                Id = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Order = blog.Order,
                ImageUrl = blogImage.ImageUrl,
                UserId = blog.UserId,
                
            };
            return View(blogViewModel);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditBlog(EditBlogViewModel blogViewModel)
        {
            if (ModelState.IsValid)
            {
                var blog = await _blogReadRepository.GetByIdAsync(blogViewModel.Id.ToString());
                if (blog == null)
                    return NotFound();
                blog.Title = blogViewModel.Title;
                if (blog.Title != blogViewModel.Title)
                {
                    blog.Title = blogViewModel.Title;
                    blog.Slug = SlugHelper.Slugify(blog.Title);
                }

                blog.Content = blogViewModel.Content;
                blog.Order = blogViewModel.Order;
                bool value = await _blogReadRepository.GetBlogsForTitle(blogViewModel.Title);
                if (value)
                    return RedirectToAction("EditBlog", "Blog");
                _blogWriteRepository.Update(blog);
                await _blogWriteRepository.SaveAsync();

                if (blogViewModel.BlogImage != null)
                {
                    var blogImage = await _blogImageReadRepository.GetBlogImageByBlogAsync(blog.Id.ToString());
                    if (blogImage != null)
                    {
                        await _blogImageWriteRepository.RemoveAsync(blogImage.Id.ToString());
                        await _imageService.DeleteImageAsync(blogImage.ImageUrl);
                        await _blogImageWriteRepository.SaveAsync();

                    }
                    string filePath = await _imageService.SaveImageAsync(blogViewModel.BlogImage, blog.Id.ToString());
                    var newBlogImage = new BlogImage
                    {
                        BlogId = blog.Id,
                        ImageUrl = filePath
                    };
                    await _blogImageWriteRepository.AddAsync(newBlogImage);
                    await _blogImageWriteRepository.SaveAsync();
                }

                return RedirectToAction("UserBlogList", "Blog");
            }
            return View(blogViewModel);
        }
        [HttpGet]
        [Authorize]
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
            await _imageService.DeleteImageAsync(blogImage.ImageUrl);
            await _blogWriteRepository.SaveAsync();

            return RedirectToAction("UserBlogList", "Blog");
        }
    }
}
