using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context) : IQueryRepository<Order>
    {
        public IQueryable<Order> GetAll()
        {
            return context.Orders;
        }

        public async Task<Order?> GetByIdAsync(params object[] id)
        {
            return await context.Orders.FindAsync(id).ConfigureAwait(false);
        }

        public int Count(Func<Order, bool> predicate)
        {
            return context.Orders.Count(predicate);
        }
    }
}
