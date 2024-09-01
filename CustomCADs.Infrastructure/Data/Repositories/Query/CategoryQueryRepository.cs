using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CategoryQueryRepository(CadContext context) : IQueryRepository<Category, int>
    {
        public IQueryable<Category> GetAll(bool asNoTracking = false)
        {
            return Query(context.Categories, asNoTracking);
        }

        public async Task<Category?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            return await Query(context.Categories, asNoTracking)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Categories.AnyAsync(o => o.Id == id).ConfigureAwait(false);

        public int Count(Func<Category, bool> predicate, bool asNoTracking = false)
        {
            return Query(context.Categories, asNoTracking)
                .Count(predicate);
        }

        private static IQueryable<Category> Query(DbSet<Category> categories, bool asNoTracking)
            => asNoTracking ? categories.AsNoTracking() : categories;
    }
}
