using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface IUserQueries
{
    IQueryable<User> GetAll(bool asNoTracking = false);
    Task<User?> GetByIdAsync(string id, bool asNoTracking = false);
    Task<User?> GetByNameAsync(string name, bool asNoTracking = false);
    Task<User?> GetByRefreshTokenAsync(string rt, bool asNoTracking = false);
    Task<bool> ExistsByIdAsync(string id);
    Task<bool> ExistsByNameAsync(string name);
    Task<int> CountAsync(Func<User, bool> predicate, bool asNoTracking = false);
}
