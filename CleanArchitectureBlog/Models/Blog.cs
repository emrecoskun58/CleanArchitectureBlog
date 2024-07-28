using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Order { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<BlogImage> BlogImages { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
    }
}
