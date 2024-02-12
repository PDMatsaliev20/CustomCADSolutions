using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public interface IRepository
    {
        public Task<T?> GetByIdAsync<T>(params object[] id) where T : class;
        
        Task AddRangeAsync<T>(params T[] entity) where T : class;
        
        public Task AddAsync<T>(T entity) where T : class;
        
        public void Delete<T>(T entity) where T : class;
        
        void DeleteRange<T>(T[] entities) where T : class;
     
        public IQueryable<T> All<T>() where T : class;
        
        public IQueryable<T> AllReadonly<T>() where T : class;
        
        public Task<int> SaveChangesAsync();
    }
}
