namespace CleanArchitectureBlog.ViewModels
{
    public class PaginatedBlogViewModel
    {
        public IEnumerable<BlogViewModel>? Blogs { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
