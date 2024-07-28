using CleanArchitectureBlog.Abstractions;
using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        public ValueTask<bool> AddAsync(T model)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> AddRangeAsync(List<T> datas)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T model)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> RemoveAsync(string id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRange(List<T> datas)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public bool Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
