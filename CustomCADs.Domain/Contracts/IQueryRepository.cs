namespace CustomCADs.Domain.Contracts
{
    public interface IQueryRepository<TEntity, TKey> where TEntity : class
    {
        IQueryable<TEntity> GetAll(bool asNoTracking = false);
        Task<TEntity?> GetByIdAsync(TKey id, bool asNoTracking = false);
        Task<bool> ExistsByIdAsync(TKey id);
        int Count(Func<TEntity, bool> predicate, bool asNoTracking = false);
    }
}
