using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Users;

public class UserQueries(ApplicationContext context) : IUserQueries
{
    public IQueryable<User> GetAll(bool asNoTracking = false)
        => context.Users
            .Query(asNoTracking);

    public async Task<User?> GetByIdAsync(string id, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.Id == id, ct)
            .ConfigureAwait(false);
    
    public async Task<User?> GetByNameAsync(string name, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.UserName == name, ct)
            .ConfigureAwait(false);
    
    public async Task<User?> GetByRefreshTokenAsync(string rt, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.RefreshToken == rt, ct)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(string id, CancellationToken ct = default)
        => await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id, ct)
            .ConfigureAwait(false);
    
    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        => await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.UserName == name, ct)
            .ConfigureAwait(false);

    public int Count(Func<User, bool> predicate)
        => context.Users
            .AsNoTracking()
            .Count(predicate);
}
