using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Reads;

public class CategoryReads(ApplicationContext context) : ICategoryReads
{
    public IQueryable<Category> GetAll(bool asNoTracking = false)
        => context.Categories
            .Query(asNoTracking);

    public async Task<Category?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Categories
            .Query(asNoTracking)
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        => await context.Categories
            .AnyAsync(c => c.Id == id, ct)
            .ConfigureAwait(false);

    public int Count(Func<Category, bool> predicate)
        => context.Categories
            .AsNoTracking()
            .Count(predicate);
}
