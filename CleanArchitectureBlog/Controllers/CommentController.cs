using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.Models;
using CleanArchitectureBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace CleanArchitectureBlog.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly IBlogWriteRepository _blogWriteRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(ICommentReadRepository commentReadRepository, ICommentWriteRepository commentWriteRepository, IBlogReadRepository blogReadRepository, IBlogWriteRepository blogWriteRepository, UserManager<ApplicationUser> userManager)
        {
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _blogReadRepository = blogReadRepository;
            _blogWriteRepository = blogWriteRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(CommentInputViewModel model)
        {
            var blog = await _blogReadRepository.GetByIdAsync(model.BlogId.ToString());
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
                var hasCommented = await _commentReadRepository.HasUserCommentedOnBlogAsync(model.BlogId, user.Id);

                if (hasCommented)
                {
                    ModelState.AddModelError("", "You have already commented on this blog post.");
                    return RedirectToAction("BlogDetail", "BlogDetail", new { slug = blog.Slug });
                }

                var comment = new Comment
                {
                    Content = model.Content,
                    BlogId = model.BlogId,
                    UserId = user.Id,
                    CreatedAt = DateTime.Now
                };

                await _commentWriteRepository.AddAsync(comment);
                await _commentWriteRepository.SaveAsync();

                return RedirectToAction("BlogDetail", "BlogDetail", new { slug = blog.Slug });
            }

            return RedirectToAction("BlogDetail", "BlogDetail", new { slug = blog.Slug });
        }
    }
}
