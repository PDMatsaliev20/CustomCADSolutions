using CustomCADs.Domain.Enums;

namespace CustomCADs.Domain.Contracts
{
    public interface IQueries<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<TEntity, bool>? customFilter = null, bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = false);
        Task<bool> ExistsByIdAsync(int id);
        Task<int> CountAsync(Func<TEntity, bool> predicate, bool asNoTracking = false);
    }
}
