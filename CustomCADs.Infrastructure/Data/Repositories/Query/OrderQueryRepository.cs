using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context) : IQueryRepository<Order>
    {
        public IQueryable<Order> GetAll(bool asNoTracking = false)
        {
            return Query(context.Orders, asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer);
        }

        public async Task<Order?> GetByIdAsync(object id, bool asNoTracking = false)
        {
            Order? order = await Query(context.Orders, asNoTracking)
                .FirstOrDefaultAsync(o => id.Equals(o.Id))
                .ConfigureAwait(false);
            
            if (order == null)
            {
                return order;
            }

            EntityEntry<Order> entry = context.Orders.Entry(order);
            await entry.Reference(o => o.Category).LoadAsync().ConfigureAwait(false);
            await entry.Reference(o => o.Buyer).LoadAsync().ConfigureAwait(false);
            await entry.Reference(o => o.Designer).LoadAsync().ConfigureAwait(false);
            await entry.Reference(o => o.Cad).LoadAsync().ConfigureAwait(false);

            return order;
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
