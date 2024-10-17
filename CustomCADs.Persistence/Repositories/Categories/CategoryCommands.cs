using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Shared;

namespace CustomCADs.Persistence.Repositories.Categories;

public class CategoryCommands(ApplicationContext context) : ICommands<Category>
{
    public async Task<Category> AddAsync(Category category, CancellationToken ct)
        => (await context.Categories.AddAsync(category, ct).ConfigureAwait(false)).Entity;
    
    public async Task AddRangeAsync(params Category[] categories)
        => await context.Categories.AddRangeAsync(categories).ConfigureAwait(false);

    public void Delete(Category entity)
        => context.Categories.Remove(entity);

    public void DeleteRange(params Category[] entities)
        => context.Categories.RemoveRange(entities);
}
