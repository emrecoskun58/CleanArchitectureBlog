using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CleanArchitectureBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogReadRepository _blogReadRepository;

        public HomeController(ILogger<HomeController> logger, IBlogReadRepository blogReadRepository)
        {
            _logger = logger;
            _blogReadRepository = blogReadRepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var blogs = await _blogReadRepository.GetBlogsAsync(pageNumber, pageSize);
            ViewData["PageSize"] = pageSize;
            return View(blogs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
