using CustomCADs.Domain.Roles;

namespace CustomCADs.Domain.Roles.Queries;

public interface IRoleQueries
{
    IQueryable<Role> GetAll(bool asNoTracking = false);
    Task<Role?> GetByIdAsync(string id, bool asNoTracking = false, CancellationToken ct = default);
    Task<Role?> GetByNameAsync(string name, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    int Count(Func<Role, bool> predicate);
}
