using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Users
{
    public class UserQueries(ApplicationContext context) : IQueries<User, string>
    {
        public IQueryable<User> GetAll(bool asNoTracking = false)
            => context.Users
                .Query(asNoTracking);

        public async Task<User?> GetByIdAsync(string id, bool asNoTracking = false)
            => await context.Users
                .Query(asNoTracking)
                .FirstOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);

        public async Task<bool> ExistsByIdAsync(string id)
            => await context.Users
                .AnyAsync(u => u.Id == id)
                .ConfigureAwait(false);
        

        public async Task<int> CountAsync(Func<User, bool> predicate, bool asNoTracking = false)
            => await context.Users
                .Query(asNoTracking)
                .CountAsync(u => predicate(u))
                .ConfigureAwait(false);
    }
}
