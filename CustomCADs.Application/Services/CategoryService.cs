using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class CategoryService(IDbTracker dbTracker,
        IQueries<Category, int> queries, 
        ICommands<Category> commands, 
        IMapper mapper) : ICategoryService
    {
        public IEnumerable<CategoryModel> GetAll(Func<CategoryModel, bool>? customFilter = null)
        {
            IQueryable<Category> queryable = queries.GetAll(true);
            queryable = queryable.Filter(customFilter == null ? null : c => customFilter(mapper.Map<CategoryModel>(c)));
            
            IEnumerable<Category> categories = [.. queryable];
            return mapper.Map<CategoryModel[]>(categories);
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
