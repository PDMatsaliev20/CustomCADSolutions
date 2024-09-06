using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class CadCommandRepository(CadContext context, IMapper mapper) : ICommandRepository<Cad>
    {
        public async Task<Cad> AddAsync(Cad cad)
        {
            PCad entity = mapper.Map<PCad>(cad);
            var entry = await context.Cads.AddAsync(entity).ConfigureAwait(false);
            return mapper.Map<Cad>(entry.Entity);
        }
        
        public async Task AddRangeAsync(params Cad[] entity) 
            => await context.Cads.AddRangeAsync(mapper.Map<PCad[]>(entity)).ConfigureAwait(false);

        public void Delete(Cad entity) 
            => context.Cads.Remove(mapper.Map<PCad>(entity));
        
        public void DeleteRange(params Cad[] entity) 
            => context.Cads.RemoveRange(mapper.Map<PCad[]>(entity));
    }
}
