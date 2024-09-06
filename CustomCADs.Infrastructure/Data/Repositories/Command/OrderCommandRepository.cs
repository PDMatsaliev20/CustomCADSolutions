using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class OrderCommandRepository(CadContext context, IMapper mapper) : ICommandRepository<Order>
    {
        public async Task<Order> AddAsync(Order order)
        {
            POrder entity = mapper.Map<POrder>(order);
            var entry = await context.Orders.AddAsync(entity).ConfigureAwait(false);
            return mapper.Map<Order>(entry.Entity);
        }

        public async Task AddRangeAsync(params Order[] entity)
            => await context.AddRangeAsync(mapper.Map<POrder[]>(entity)).ConfigureAwait(false);

        public void Delete(Order entity)
            => context.Orders.Remove(mapper.Map<POrder>(entity));

        public void DeleteRange(params Order[] entity)
            => context.Orders.RemoveRange(mapper.Map<POrder[]>(entity));
    }
}
