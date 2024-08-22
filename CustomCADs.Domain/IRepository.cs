using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain
{
    public interface IRepository
    {
        IQueryable<T> All<T>() where T : class;

        IQueryable<T> AllReadonly<T>() where T : class;

        Task<T?> GetByIdAsync<T>(params object[] id) where T : class;

        int Count<T>(Func<T, bool> predicate) where T : class;

        Task AddRangeAsync<T>(params T[] entity) where T : class;

        Task<TKey> AddAsync<T, TKey>(T entity) where T : class, IEntity<TKey>;

        void Delete<T>(T entity) where T : class;

        void DeleteRange<T>(T[] entities) where T : class;

        Task<int> SaveChangesAsync();
    }
}
