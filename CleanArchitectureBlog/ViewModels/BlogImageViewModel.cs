using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class BlogImageViewModel
    {
        public Guid Id { get; set; }
        public string BlogId { get; set; }
        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string ImageUrl { get; set; }
    }
}
