using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CadQueryRepository(CadContext context) : IQueryRepository<Cad, int>
    {
        public IQueryable<Cad> GetAll(bool asNoTracking = false)
        {
            return Query(context.Cads, asNoTracking)
                .Include(c => c.Category)
                .Include(c => c.Creator)
                .AsSplitQuery();
        }

        public async Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            return await Query(context.Cads, asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Creator)
                .Include(o => o.Orders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Cads.AnyAsync(o => o.Id == id).ConfigureAwait(false);

        public int Count(Func<Cad, bool> predicate, bool asNoTracking = false)
        {
            return Query(context.Cads, asNoTracking)
                .Include(c => c.Creator)
                .Count(predicate);
        }

        private static IQueryable<Cad> Query(DbSet<Cad> cads, bool asNoTracking)
            => asNoTracking ? cads.AsNoTracking() : cads;
    }
}
