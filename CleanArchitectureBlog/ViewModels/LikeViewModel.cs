using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureBlog.ViewModels
{
    public class LikeViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
    }
}
