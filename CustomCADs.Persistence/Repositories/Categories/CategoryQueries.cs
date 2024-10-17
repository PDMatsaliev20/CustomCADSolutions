using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Categories;

public class CategoryQueries(ApplicationContext context) : ICategoryQueries
{
    public IQueryable<Category> GetAll(bool asNoTracking = false)
        => context.Categories
            .Query(asNoTracking);

    public async Task<Category?> GetByIdAsync(int id, bool asNoTracking = false)
        => await context.Categories
            .Query(asNoTracking)
            .FirstOrDefaultAsync(c => c.Id == id)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(int id)
        => await context.Categories
            .AnyAsync(c => c.Id == id)
            .ConfigureAwait(false);

    public int Count(Func<Category, bool> predicate)
        => context.Categories
            .AsNoTracking()
            .Count(predicate);
}
