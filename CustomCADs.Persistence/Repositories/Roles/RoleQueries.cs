using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Queries;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Roles;

public class RoleQueries(ApplicationContext context) : IRoleQueries
{
    public IQueryable<Role> GetAll(bool asNoTracking = false)
        => context.Roles
            .Query(asNoTracking);

    public async Task<Role?> GetByIdAsync(string id, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Roles
            .Query(asNoTracking)
            .FirstOrDefaultAsync(r => r.Id == id, ct)
            .ConfigureAwait(false);
    
    public async Task<Role?> GetByNameAsync(string name, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Roles
            .Query(asNoTracking)
            .FirstOrDefaultAsync(r => r.Name == name, ct)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default)
    => await context.Roles
            .AnyAsync(r => r.Id == id, ct)
            .ConfigureAwait(false);
    
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
    => await context.Roles
            .AnyAsync(r => r.Name == name, ct)
            .ConfigureAwait(false);

    public int Count(Func<Role, bool> predicate)
        => context.Roles
            .AsNoTracking()
            .Count(predicate);
}
