using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context) : IQueryRepository<Order>
    {
        public IQueryable<Order> GetAll(bool asNoTracking = false)
        {
            return Query(context.Orders, asNoTracking)
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer);
        }

        public async Task<Order?> GetByIdAsync(object id, bool asNoTracking = false)
        {
            return await Query(context.Orders, asNoTracking)
                .AsNoTracking()
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .Include(o => o.Cad)
                .FirstOrDefaultAsync(o => id.Equals(o.Id)).ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(object id)
            => await context.Orders.AnyAsync(o => id.Equals(o.Id)).ConfigureAwait(false);

        public int Count(Func<Order, bool> predicate, bool asNoTracking = false)
        {
            return Query(context.Orders, asNoTracking)
                .Include(o => o.Buyer)
                .Count(predicate);
        }

        private static IQueryable<Order> Query(DbSet<Order> orders, bool asNoTracking)
            => asNoTracking ? orders.AsNoTracking() : orders;
    }
}
