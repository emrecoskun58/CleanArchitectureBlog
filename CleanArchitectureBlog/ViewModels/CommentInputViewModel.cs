using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class CommentInputViewModel
    {
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public Guid BlogId { get; set; }
    }
}
