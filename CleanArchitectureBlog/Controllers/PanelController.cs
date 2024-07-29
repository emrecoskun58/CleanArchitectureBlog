using CleanArchitectureBlog.Abstractions.Repositories.BlogImageRepository;
using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
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
        private readonly IBlogImageWriteRepository _blogImageWriteRepository;
        private readonly IImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PanelController(IBlogWriteRepository blogWriteRepository, IBlogImageWriteRepository blogImageWriteRepository, IImageService imageService, UserManager<ApplicationUser> userManager)
        {
            _blogWriteRepository = blogWriteRepository;
            _blogImageWriteRepository = blogImageWriteRepository;
            _imageService = imageService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
