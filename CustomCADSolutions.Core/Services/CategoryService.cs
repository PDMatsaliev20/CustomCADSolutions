using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository repository;

        public CategoryService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync<Category>(id) ?? throw new KeyNotFoundException();
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return (await GetAllAsync()).FirstOrDefault(c => c.Name == name) ?? throw new KeyNotFoundException();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await repository.All<Category>().ToArrayAsync();
        }
        public async Task<int> CreateAsync(Category entity)
        {
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            return entity.Id;
        }

        public async Task CreateRangeAsync(params Category[] entities)
        {
            await repository.AddRangeAsync(entities);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(Category model)
        {
            Category entity = await repository
                .GetByIdAsync<Category>(model.Id)
                ?? throw new KeyNotFoundException();

            entity.Id = model.Id;
            entity.Name = model.Name;
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Category entity = await repository
                .GetByIdAsync<Category>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete(entity);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(params int[] ids)
        {
            Category[] entities = await repository
                .All<Category>()
                .Where(c => ids.Any(id => c.Id == id))
                .ToArrayAsync();

            repository.DeleteRange(entities);
            await repository.SaveChangesAsync();
        }

    }
}
