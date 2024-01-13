using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<T?> GetByIdAsync<T>(params int[] id) where T : class
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            // Cannot access a disposed context instance. A common cause of this error is disposing a context instance that was resolved from dependency injection and then later trying to use the same context instance elsewhere in your application. This may occur if you are calling 'Dispose' on the context instance, or wrapping it in a using statement. If you are using dependency injection, you should let the dependency injection container take care of disposing context instances. Object name: 'CADContext'.
            return await context.SaveChangesAsync();
        }
    }
}
