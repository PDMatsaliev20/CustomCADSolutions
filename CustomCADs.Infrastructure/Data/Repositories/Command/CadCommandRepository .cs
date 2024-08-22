using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class CadCommandRepository(CadContext context) : ICommandRepository<Cad>
    {
        public async Task<Cad> AddAsync(Cad entity) 
            => (await context.AddAsync(entity).ConfigureAwait(false)).Entity;
        
        public async Task AddRangeAsync(params Cad[] entity) 
            => await context.AddRangeAsync(entity).ConfigureAwait(false);

        public void Delete(Cad entity) 
            => context.Remove(entity);
        
        public void DeleteRange(params Cad[] entity) 
            => context.RemoveRange(entity);
    }
}
