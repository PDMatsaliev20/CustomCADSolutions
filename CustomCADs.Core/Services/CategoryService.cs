using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Models;
using CustomCADs.Infrastructure.Data.Common;
using CustomCADs.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Core.Services
{
    public class CategoryService(IRepository repository) : ICategoryService
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg
                    => cfg.AddProfile<CategoryCoreProfile>())
                .CreateMapper();

        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            Category[] entities = await repository.All<Category>().ToArrayAsync();
            CategoryModel[] models = mapper.Map<CategoryModel[]>(entities);
            return models;
        }

        public async Task<IEnumerable<string>> GetAllNamesAsync()
        {
            return await repository.All<Category>().Select(c => c.Name).ToArrayAsync();
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            Category entity = await repository.GetByIdAsync<Category>(id)
                ?? throw new KeyNotFoundException();

            CategoryModel model = mapper.Map<CategoryModel>(entity);
            return model;
        }
 
        public async Task<int> CreateAsync(CategoryModel model)
        {
            Category entity = mapper.Map<Category>(model);  
            
            EntityEntry<Category> category = await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
            
            return category.Entity.Id;
        }

        public async Task EditAsync(int id, CategoryModel model)
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
