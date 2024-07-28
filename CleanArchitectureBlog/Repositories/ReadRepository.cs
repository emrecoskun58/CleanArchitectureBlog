using CleanArchitectureBlog.Abstractions;
using CleanArchitectureBlog.Models.Common;
using System.Linq.Expressions;

namespace CleanArchitectureBlog.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        public IQueryable<T> GetAll(bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> GetByIdAsync(string id, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public ValueTask<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }
}
