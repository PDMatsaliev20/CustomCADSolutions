using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Roles
{
    public class RoleQueries(ApplicationContext context) : IQueries<Role, string>
    {
        public IQueryable<Role> GetAll(bool asNoTracking = false)
            => context.Roles
                .Query(asNoTracking);

        public async Task<Role?> GetByIdAsync(string id, bool asNoTracking = false)
            => await context.Roles
                .Query(asNoTracking)
                .FirstOrDefaultAsync(r => r.Id == id)
                .ConfigureAwait(false);

        public async Task<bool> ExistsByIdAsync(string id)
        => await context.Roles
                .AnyAsync(r => r.Id == id)
                .ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Role, bool> predicate, bool asNoTracking = false)
            => await context.Roles
                .Query(asNoTracking)
                .CountAsync(r => predicate(r))
                .ConfigureAwait(false);
    }
}
