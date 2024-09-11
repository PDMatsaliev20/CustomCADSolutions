using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Roles
{
    public class RoleQueries(ApplicationContext context) : IQueries<Role, string>
    {
        public async Task<IEnumerable<Role>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Role, bool>? customFilter = null, bool asNoTracking = false)
        {
            IQueryable<Role> query = context.Roles.Query(asNoTracking);

            Role[] roles = await query
                .ToArrayAsync()
                .ConfigureAwait(false);

            return customFilter != null ? roles.Where(customFilter) : roles;
        }

        public async Task<Role?> GetByIdAsync(string id, bool asNoTracking = false)
        {
            Role? role = await context.Roles
                .Query(asNoTracking)
                .FirstOrDefaultAsync(r => r.Id == id)
                .ConfigureAwait(false);

            return role;
        }

        public async Task<bool> ExistsByIdAsync(string id)
        {
            return await context.Roles
                .AnyAsync(r => r.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Func<Role, bool> predicate, bool asNoTracking = false)
        {
            int count = await context.Roles
                .Query(asNoTracking)
                .CountAsync(r => predicate(r))
                .ConfigureAwait(false);

            return count;
        }
    }
}
