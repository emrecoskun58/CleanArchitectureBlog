using CleanArchitectureBlog.Abstractions.Repositories.CommentRepository;
using CleanArchitectureBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureBlog.ViewComponents
{
    public class AddCommentViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Guid BlogId)
        {
            var model = new CommentInputViewModel { BlogId = BlogId };
            return View(model);
        }
    }
}
