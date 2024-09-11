using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Users
{
    public class UserQueries(ApplicationContext context) : IQueries<User, string>
    {
        public async Task<IEnumerable<User>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<User, bool>? customFilter = null, bool asNoTracking = false)
        {
            User[] users = await context.Users
                .Query(asNoTracking)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return customFilter != null ? users.Where(customFilter) : users;
        }

        public async Task<User?> GetByIdAsync(string id, bool asNoTracking = false)
        {
            User? user = await context.Users
                .Query(asNoTracking)
                .FirstOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);

            return user;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await context.Users
                .AnyAsync(u => u.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Func<User, bool> predicate, bool asNoTracking = false)
        {
            return await context.Users
                .Query(asNoTracking)
                .CountAsync(u => predicate(u))
                .ConfigureAwait(false);
        }
    }
}
