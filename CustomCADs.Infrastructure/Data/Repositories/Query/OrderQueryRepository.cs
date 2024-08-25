using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context) : IQueryRepository<Order>
    {
        public IQueryable<Order> GetAll()
        {
            return context.Orders
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer);
        }

        public async Task<Order?> GetByIdAsync(object id)
        {
            return await context.Orders
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .Include(o => o.Cad)
                .FirstOrDefaultAsync(o => id.Equals(o.Id)).ConfigureAwait(false);
        }

        public int Count(Func<Order, bool> predicate)
        {
            return context.Orders
                .Include(o => o.Buyer)
                .Count(predicate);
        }
    }
}
