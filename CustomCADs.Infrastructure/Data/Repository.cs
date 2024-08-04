using CustomCADs.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Infrastructure.Data
{
    public class Repository(CadContext context) : IRepository
    {
        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>();
        }

        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }

        public async Task<T?> GetByIdAsync<T>(params object[] ids) where T : class
        {
            return await context.Set<T>().FindAsync(ids).ConfigureAwait(false);
        }

        public int Count<T>(Func<T, bool> predicate) where T : class
        {
            return context.Set<T>().Count(predicate);
        }

        public async Task<EntityEntry<T>> AddAsync<T>(T entity) where T : class
        {
            return await context.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }

        public async Task AddRangeAsync<T>(params T[] entity) where T : class
        {
            await context.Set<T>().AddRangeAsync(entity).ConfigureAwait(false);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync() 
            => await context.SaveChangesAsync().ConfigureAwait(false);
    }
}
