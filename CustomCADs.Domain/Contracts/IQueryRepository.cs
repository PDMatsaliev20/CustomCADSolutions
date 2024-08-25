namespace CustomCADs.Domain.Contracts
{
    public interface IQueryRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(object id);
        int Count(Func<T, bool> predicate);
    }
}
