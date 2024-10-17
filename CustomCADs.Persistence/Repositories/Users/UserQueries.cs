using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Users;

public class UserQueries(ApplicationContext context) : IUserQueries
{
    public IQueryable<User> GetAll(bool asNoTracking = false)
        => context.Users
            .Query(asNoTracking);

    public async Task<User?> GetByIdAsync(string id, bool asNoTracking = false)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.Id == id)
            .ConfigureAwait(false);
    
    public async Task<User?> GetByNameAsync(string name, bool asNoTracking = false)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.UserName == name)
            .ConfigureAwait(false);
    
    public async Task<User?> GetByRefreshTokenAsync(string rt, bool asNoTracking = false)
        => await context.Users
            .Query(asNoTracking)
            .FirstOrDefaultAsync(u => u.RefreshToken == rt)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(string id)
        => await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == id)
            .ConfigureAwait(false);
    
    public async Task<bool> ExistsByNameAsync(string name)
        => await context.Users
            .AsNoTracking()
            .AnyAsync(u => u.UserName == name)
            .ConfigureAwait(false);

    public int Count(Func<User, bool> predicate)
        => context.Users
            .AsNoTracking()
            .Count(predicate);
}
