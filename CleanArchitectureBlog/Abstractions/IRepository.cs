using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Abstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
    }
}
