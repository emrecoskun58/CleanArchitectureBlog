using CleanArchitectureBlog.Abstractions;
using CleanArchitectureBlog.Models.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using CleanArchitectureBlog.Contexts;

namespace CleanArchitectureBlog.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly CleanArchitectureDbContext _context;

        public WriteRepository(CleanArchitectureDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();

        public async ValueTask<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async ValueTask<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public bool Remove(T model)
        {
            model.IsActive = false;
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }

        public async ValueTask<bool> RemoveAsync(string id)
        {
            T model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(model);
        }

        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async ValueTask<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
