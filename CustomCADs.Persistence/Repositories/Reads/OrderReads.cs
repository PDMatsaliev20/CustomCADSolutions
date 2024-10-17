using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Reads;

public class OrderReads(ApplicationContext context) : IOrderReads
{
    public IQueryable<Order> GetAll(bool asNoTracking = false)
        => context.Orders
            .Query(asNoTracking)
            .Include(o => o.Category)
            .Include(o => o.Buyer)
            .Include(o => o.Designer)
            .AsSplitQuery();

    public async Task<Order?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Orders
            .Query(asNoTracking)
            .Include(o => o.Category)
            .Include(o => o.Buyer)
            .Include(o => o.Designer)
            .Include(o => o.Cad)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        => await context.Orders
            .AnyAsync(o => o.Id == id, ct)
            .ConfigureAwait(false);

    public int Count(Func<Order, bool> predicate)
        => context.Orders
            .AsNoTracking()
            .Include(o => o.Buyer)
            .Count(predicate);
}
