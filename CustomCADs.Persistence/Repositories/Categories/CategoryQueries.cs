using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Categories
{
    public class CategoryQueries(ApplicationContext context) : IQueries<Category, int>
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

        public async Task<int> CountAsync(Func<Category, bool> predicate, bool asNoTracking = false)
            => await context.Categories
                .Query(asNoTracking)
                .CountAsync(c => predicate(c))
                .ConfigureAwait(false);
    }
}
