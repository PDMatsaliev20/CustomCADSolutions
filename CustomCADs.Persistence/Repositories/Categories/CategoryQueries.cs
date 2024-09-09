using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Categories
{
    public class CategoryQueries(ApplicationContext context) : IQueries<Category>
    {
        public async Task<IEnumerable<Category>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Category, bool>? customFilter = null, bool asNoTracking = false)
        {
            Category[] categories = await context.Categories
                .Query(asNoTracking)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return categories;
        }

        public async Task<Category?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            Category? category = await context.Categories
                .Query(asNoTracking)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            return category;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await context.Categories.AnyAsync(c => c.Id == id).ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Func<Category, bool> predicate, bool asNoTracking = false)
        {
            Category[] categories = await context.Categories
                .Query(asNoTracking)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return categories.Count(predicate);
        }
    }
}
