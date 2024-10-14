using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class CategoryService(IUnitOfWork unitOfWork,
        ICategoryQueries queries, 
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
                ?? throw new CategoryNotFoundException(id);

            CategoryModel model = mapper.Map<CategoryModel>(entity);
            return model;
        }
        
        public async Task<bool> ExistsByIdAsync(int id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);

        public async Task<int> CreateAsync(CategoryModel model)
        {
            Category entity = mapper.Map<Category>(model);  
            
            await commands.AddAsync(entity).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            
            return entity.Id;
        }

        public async Task EditAsync(int id, CategoryModel model)
        {
            Category entity = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new CategoryNotFoundException(id);

            entity.Name = model.Name;

            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Category entity = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new CategoryNotFoundException(id);

            commands.Delete(entity);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
