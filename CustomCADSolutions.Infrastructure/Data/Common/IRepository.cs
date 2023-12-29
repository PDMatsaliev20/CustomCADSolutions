using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public interface IRepository
    {
        public Task<T?> GetByIdAsync<T>(int id) where T : class;
        
        public Task AddAsync<T>(T entity) where T : class;
        
        public void Delete<T>(T entity) where T : class;
        
        public IQueryable<T> All<T>() where T : class;
        
        public IQueryable<T> AllReadonly<T>() where T : class;
        
        public Task<int> SaveChangesAsync();
    }
}
