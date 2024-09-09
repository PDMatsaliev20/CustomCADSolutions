using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Persistence.Repositories.Orders
{
    public class OrderCommands(ApplicationContext context) : ICommands<Order>
    {
        public async Task<Order> AddAsync(Order order)
            => (await context.Orders.AddAsync(order).ConfigureAwait(false)).Entity;
        
        public async Task AddRangeAsync(params Order[] orders)
            => await context.Orders.AddRangeAsync(orders).ConfigureAwait(false);

        public void Delete(Order order)
            => context.Orders.Remove(order);

        public void DeleteRange(params Order[] orders)
            => context.Orders.RemoveRange(orders);
    }
}
