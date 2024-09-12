using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Cads
{
    public class CadQueries(ApplicationContext context) : IQueries<Cad, int>
    {
        public IQueryable<Cad> GetAll(bool asNoTracking = false)
            => context.Cads
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Creator)
                .AsSplitQuery();

        public async Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false)
            => await context.Cads
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Creator)
                .Include(o => o.Orders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Cads
                .AnyAsync(o => o.Id == id)
                .ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Cad, bool> predicate, bool asNoTracking = false)
            => await context.Cads
                .Query(asNoTracking)
                .Include(c => c.Creator)
                .CountAsync(c => predicate(c))
                .ConfigureAwait(false);
    }
}
