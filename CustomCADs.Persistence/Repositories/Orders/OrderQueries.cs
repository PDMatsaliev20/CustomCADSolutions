using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Orders
{
    public class OrderQueries(ApplicationContext context) : IOrderQueries
    {
        public IQueryable<Order> GetAll(bool asNoTracking = false)
            => context.Orders
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery();

        public async Task<Order?> GetByIdAsync(int id, bool asNoTracking = false)
            => await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Orders
                .AnyAsync(o => o.Id == id)
                .ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Order, bool> predicate, bool asNoTracking = false)
            => await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Buyer)
                .CountAsync(c => predicate(c))
                .ConfigureAwait(false);
    }
}
