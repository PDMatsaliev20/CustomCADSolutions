using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface IRoleQueries
{
    IQueryable<Role> GetAll(bool asNoTracking = false);
    Task<Role?> GetByIdAsync(string id, bool asNoTracking = false, CancellationToken ct = default);
    Task<Role?> GetByNameAsync(string name, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    int Count(Func<Role, bool> predicate);
}
