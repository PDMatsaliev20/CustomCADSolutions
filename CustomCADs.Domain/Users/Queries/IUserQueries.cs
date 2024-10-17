using CustomCADs.Domain.Users;

namespace CustomCADs.Domain.Users.Queries;

public interface IUserQueries
{
    IQueryable<User> GetAll(bool asNoTracking = false);
    Task<User?> GetByIdAsync(string id, bool asNoTracking = false, CancellationToken ct = default);
    Task<User?> GetByNameAsync(string name, bool asNoTracking = false, CancellationToken ct = default);
    Task<User?> GetByRefreshTokenAsync(string rt, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    int Count(Func<User, bool> predicate);
}
