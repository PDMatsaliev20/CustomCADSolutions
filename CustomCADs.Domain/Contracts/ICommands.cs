namespace CustomCADs.Domain.Contracts;

public interface ICommands<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task AddRangeAsync(params T[] entities);
    void Delete(T entity);
    void DeleteRange(params T[] entities);
}
