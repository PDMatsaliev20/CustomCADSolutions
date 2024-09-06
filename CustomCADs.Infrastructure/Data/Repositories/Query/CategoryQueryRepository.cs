using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CategoryQueryRepository(CadContext context, IMapper mapper) : IQueryRepository<Category, int>
    {
        public async Task<IEnumerable<Category>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Category, bool>? customFilter = null, bool asNoTracking = false)
        {
            PCategory[] categories = await context.Categories
                .Query(asNoTracking)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return categories.Select(mapper.Map<Category>);
        }

        public async Task<Category?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            PCategory? category = await context.Categories
                .Query(asNoTracking)
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            return mapper.Map<Category?>(category);
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Categories.AnyAsync(o => o.Id == id).ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Category, bool> predicate, bool asNoTracking = false)
        {
            PCategory[] categories = await context.Categories
                .Query(asNoTracking)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return categories.Count(c => predicate(mapper.Map<Category>(c)));
        }
    }
}
