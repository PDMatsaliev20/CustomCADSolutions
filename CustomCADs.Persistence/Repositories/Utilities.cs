using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories
{
    public static class Utilities
    {
        public static IQueryable<T> Query<T>(this DbSet<T> set, bool asNoTracking) where T : class
            => asNoTracking ? set.AsNoTracking() : set;
    }
}
