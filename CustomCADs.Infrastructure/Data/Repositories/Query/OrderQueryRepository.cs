using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context) : IQueryRepository<Order, int>
    {
        public IQueryable<Order> GetAll(bool asNoTracking = false)
        {
            return Query(context.Orders, asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery();
        }

        public async Task<Order?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            return await Query(context.Orders, asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery()
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Orders.AnyAsync(o => o.Id == id).ConfigureAwait(false);

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
