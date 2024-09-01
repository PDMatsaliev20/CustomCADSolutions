using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Application.Services
{
    public class CategoryService(IDbTracker dbTracker,
        IQueryRepository<Category, int> queries, 
        ICommandRepository<Category> commands, 
        IMapper mapper) : ICategoryService
    {
        public async Task<IEnumerable<CategoryModel>> GetAllAsync()
        {
            Category[] entities = await queries.GetAll(asNoTracking: true).ToArrayAsync().ConfigureAwait(false);
            CategoryModel[] models = mapper.Map<CategoryModel[]>(entities);
            return models;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            Category entity = await queries.GetByIdAsync(id, asNoTracking: true)
                .ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            CategoryModel model = mapper.Map<CategoryModel>(entity);
            return model;
        }

        public async Task<int> CreateAsync(CategoryModel model)
        {
            Category entity = mapper.Map<Category>(model);  
            
            await commands.AddAsync(entity).ConfigureAwait(false);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
            
            return entity.Id;
        }

        public async Task EditAsync(int id, CategoryModel model)
        {
            Category entity = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            entity.Name = model.Name;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Category entity = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            commands.Delete(entity);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
