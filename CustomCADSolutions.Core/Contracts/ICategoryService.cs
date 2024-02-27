using CustomCADSolutions.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Contracts
{
    public interface ICategoryService
    {
        Task<Category> GetByIdAsync(int id);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<int> CreateAsync(Category entity);

        Task CreateRangeAsync(params Category[] entities);

        Task EditAsync(Category entity);

        Task DeleteAsync(int id);

        Task DeleteRangeAsync(params int[] ids);
    }
}
