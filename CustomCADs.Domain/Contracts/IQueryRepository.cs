using CustomCADs.Domain.Enums;

namespace CustomCADs.Domain.Contracts
{
    public interface IQueryRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<TEntity, bool>? customFilter = null, bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(TKey id, bool asNoTracking = false);
        Task<bool> ExistsByIdAsync(TKey id);
        Task<int> CountAsync(Func<TEntity, bool> predicate, bool asNoTracking = false);
    }
}
