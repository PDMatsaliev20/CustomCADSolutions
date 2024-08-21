using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Application.Services
{
    public class CategoryService(IRepository repository, IMapper mapper) : ICategoryService
    {
        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            Category[] entities = await repository.All<Category>().ToArrayAsync().ConfigureAwait(false);
            CategoryModel[] models = mapper.Map<CategoryModel[]>(entities);
            return models;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            Category entity = await repository.GetByIdAsync<Category>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            CategoryModel model = mapper.Map<CategoryModel>(entity);
            return model;
        }

        public async Task<int> CreateAsync(CategoryModel model)
        {
            Category entity = mapper.Map<Category>(model);  
            
            EntityEntry<Category> category = await repository.AddAsync(entity).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);
            
            return category.Entity.Id;
        }

        public async Task EditAsync(int id, CategoryModel model)
        {
            Category entity = await repository.GetByIdAsync<Category>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            entity.Name = model.Name;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Category entity = await repository.GetByIdAsync<Category>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            repository.Delete(entity);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
