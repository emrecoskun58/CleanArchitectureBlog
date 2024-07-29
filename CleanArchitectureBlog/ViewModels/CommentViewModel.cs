using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
    }
}
