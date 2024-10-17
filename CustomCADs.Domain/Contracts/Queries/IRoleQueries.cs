using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface IRoleQueries
{
    IQueryable<Role> GetAll(bool asNoTracking = false);
    Task<Role?> GetByIdAsync(string id, bool asNoTracking = false);
    Task<Role?> GetByNameAsync(string name, bool asNoTracking = false);
    Task<bool> ExistsByIdAsync(string id);
    Task<bool> ExistsByNameAsync(string name);
    int Count(Func<Role, bool> predicate);
}
