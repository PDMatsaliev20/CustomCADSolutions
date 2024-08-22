using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class OrderCommandRepository(CadContext context) : ICommandRepository<Order>
    {
        public async Task<Order> AddAsync(Order entity) 
            => (await context.AddAsync(entity).ConfigureAwait(false)).Entity;
        
        public async Task AddRangeAsync(params Order[] entity) 
            => await context.AddRangeAsync(entity).ConfigureAwait(false);

        public void Delete(Order entity) 
            => context.Remove(entity);
        
        public void DeleteRange(params Order[] entity) 
            => context.RemoveRange(entity);
    }
}
