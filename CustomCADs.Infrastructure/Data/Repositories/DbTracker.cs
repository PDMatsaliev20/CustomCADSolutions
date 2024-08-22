using CustomCADs.Domain.Contracts;

namespace CustomCADs.Infrastructure.Data.Repositories
{
    public class DbTracker(CadContext context) : IDbTracker
    {
        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync().ConfigureAwait(false);
    }
}
