using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence
{
    public static class Utils
    {
        public static IQueryable<T> Query<T>(this DbSet<T> set, bool asNoTracking) where T : class
            => asNoTracking ? set.AsNoTracking() : set;
    }
}
