namespace CustomCADs.Domain.Contracts
{
    public interface IQueries<TEntity, TKey> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(TKey id, bool asNoTracking = false);
        Task<bool> ExistsByIdAsync(TKey id);
        Task<int> CountAsync(Func<TEntity, bool> predicate, bool asNoTracking = false);
    }
}
