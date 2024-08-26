namespace CustomCADs.Domain.Contracts
{
    public interface IQueryRepository<T> where T : class
    {
        IQueryable<T> GetAll(bool asNoTracking = false);
        Task<T?> GetByIdAsync(object id, bool asNoTracking = false);
        Task<bool> ExistsByIdAsync(object id);
        int Count(Func<T, bool> predicate, bool asNoTracking = false);
    }
}
