using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADSolutions.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;

        public CategoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await repository.All<Category>().ToArrayAsync();
        }

        public async Task<IEnumerable<string>> GetAllNamesAsync()
        {
            return await repository.All<Category>().Select(c => c.Name).ToArrayAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            Category category = await repository.GetByIdAsync<Category>(id)
                ?? throw new KeyNotFoundException();

            return category;
        }
 
        public async Task<int> CreateAsync(Category entity)
        {
            EntityEntry<Category> category = await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            return category.Entity.Id;
        }

        public async Task EditAsync(int id, Category model)
        {
            Category entity = await repository.GetByIdAsync<Category>(id)
                ?? throw new KeyNotFoundException();

            entity.Name = model.Name;
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category entity = await repository.GetByIdAsync<Category>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete(entity);
            await repository.SaveChangesAsync();
        }
    }
}
