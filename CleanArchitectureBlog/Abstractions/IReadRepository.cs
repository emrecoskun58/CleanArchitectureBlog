using CleanArchitectureBlog.Models.Common;
using System.Linq.Expressions;

namespace CleanArchitectureBlog.Abstractions
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        ValueTask<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        ValueTask<T> GetByIdAsync(string id, bool tracking = true);
    }
}
