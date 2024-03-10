using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public class Repository : IRepository
    {
        private readonly CADContext context;

        public Repository(CADContext context)
        {
            this.context = context;
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync<T>(params T[] entity) where T : class
        {
            await context.Set<T>().AddRangeAsync(entity); 
        }

        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>();
        }

        public IQueryable<T> AllReadonly<T>() where T : class
        {
            return context.Set<T>().AsNoTracking();
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            context.Set<T>().RemoveRange(entities);
        }

        public async Task<T?> GetByIdAsync<T>(params object[] ids) where T : class
        {
            return await context.Set<T>().FindAsync(ids);
        }

        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
