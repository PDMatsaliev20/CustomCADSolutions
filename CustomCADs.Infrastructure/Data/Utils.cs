using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data
{
    public static class Utils
    {
        public static IQueryable<T> Query<T>(this DbSet<T> set, bool asNoTracking) where T : class
            => asNoTracking ? set.AsNoTracking() : set;
    }
}
