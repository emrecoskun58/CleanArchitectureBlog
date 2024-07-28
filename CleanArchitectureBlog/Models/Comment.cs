using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Models
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
