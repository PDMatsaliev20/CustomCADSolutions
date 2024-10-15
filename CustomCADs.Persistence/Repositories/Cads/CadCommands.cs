using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Persistence.Repositories.Cads;

public class CadCommands(ApplicationContext context) : ICommands<Cad>
{
    public async Task<Cad> AddAsync(Cad cad)
        => (await context.Cads.AddAsync(cad).ConfigureAwait(false)).Entity;
    
    public async Task AddRangeAsync(params Cad[] cads)
        => await context.Cads.AddRangeAsync(cads).ConfigureAwait(false);

    public void Delete(Cad cad)
        => context.Cads.Remove(cad);

    public void DeleteRange(params Cad[] cads)
        => context.Cads.RemoveRange(cads);
}
