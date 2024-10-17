using CustomCADs.Domain.Shared;

namespace CustomCADs.Persistence.Repositories;

public class Writes<TEntity>(ApplicationContext context) : IWrites<TEntity> where TEntity : class
{
    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
        => (await context.Set<TEntity>().AddAsync(entity, ct).ConfigureAwait(false)).Entity;

    public async Task AddRangeAsync(CancellationToken ct = default, params TEntity[] entities)
        => await context.Set<TEntity>().AddRangeAsync(entities, ct).ConfigureAwait(false);

    public void Delete(TEntity entity)
        => context.Set<TEntity>().Remove(entity);

    public void DeleteRange(params TEntity[] entities)
        => context.Set<TEntity>().RemoveRange(entities);
}
