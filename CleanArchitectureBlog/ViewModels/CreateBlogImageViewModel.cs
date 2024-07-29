using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class CreateBlogImageViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public IFormFile ImageUrl { get; set; }
    }
}
