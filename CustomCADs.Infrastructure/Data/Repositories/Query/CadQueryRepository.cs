using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CadQueryRepository(CadContext context) : IQueryRepository<Cad>
    {
        public IQueryable<Cad> GetAll(bool asNoTracking = false)
        {
            return Query(context.Cads, asNoTracking)
                .Include(c => c.Category)
                .Include(c => c.Creator);
        }

        public async Task<Cad?> GetByIdAsync(object id, bool asNoTracking = false)
        {
            Cad? cad = await Query(context.Cads, asNoTracking)
                .FirstOrDefaultAsync(c => id.Equals(c.Id))
                .ConfigureAwait(false);
            
            if (cad == null) 
            { 
                return null; 
            }

            EntityEntry<Cad> entry = context.Cads.Entry(cad);
            await entry.Reference(o => o.Category).LoadAsync().ConfigureAwait(false);
            await entry.Reference(o => o.Creator).LoadAsync().ConfigureAwait(false);
            await entry.Reference(o => o.Orders).LoadAsync().ConfigureAwait(false);

            return cad;
        }

        public async Task<bool> ExistsByIdAsync(object id)
            => await context.Cads.AnyAsync(o => id.Equals(o.Id)).ConfigureAwait(false);

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
