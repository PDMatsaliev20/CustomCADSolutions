using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Persistence.Repositories.Categories
{
    public class CategoryCommands(ApplicationContext context) : ICommands<Category>
    {
        public async Task<Category> AddAsync(Category category)
            => (await context.Categories.AddAsync(category).ConfigureAwait(false)).Entity;
        
        public async Task AddRangeAsync(params Category[] categories)
            => await context.Categories.AddRangeAsync(categories).ConfigureAwait(false);

        public void Delete(Category entity)
            => context.Categories.Remove(entity);

        public void DeleteRange(params Category[] entities)
            => context.Categories.RemoveRange(entities);
    }
}
