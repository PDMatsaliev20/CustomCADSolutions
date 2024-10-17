namespace CustomCADs.Domain.Shared;

public interface IWrites<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default);
    Task AddRangeAsync(CancellationToken ct = default, params TEntity[] entities);
    void Delete(TEntity entity);
    void DeleteRange(params TEntity[] entities);
}
