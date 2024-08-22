namespace CustomCADs.Domain.Contracts
{
    public interface ICommandRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(params T[] entity);
        void Delete(T entity);
        void DeleteRange(params T[] entity);
    }
}
