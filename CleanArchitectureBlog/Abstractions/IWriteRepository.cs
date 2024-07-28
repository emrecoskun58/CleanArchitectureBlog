using CleanArchitectureBlog.Models.Common;

namespace CleanArchitectureBlog.Abstractions
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        ValueTask<bool> AddAsync(T model);
        ValueTask<bool> AddRangeAsync(List<T> datas);
        bool Remove(T model);
        bool RemoveRange(List<T> datas);
        ValueTask<bool> RemoveAsync(string id);
        bool Update(T model);
        ValueTask<int> SaveAsync();
    }
}
