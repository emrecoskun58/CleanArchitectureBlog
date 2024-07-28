using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Models
{
    public class BlogImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
