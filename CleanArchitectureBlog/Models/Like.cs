using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Models
{
    public class Like : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
