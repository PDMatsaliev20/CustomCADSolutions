using CustomCADs.Domain.Contracts;

namespace CustomCADs.Persistence.Repositories
{
    public class DbTracker(ApplicationContext context) : IDbTracker
    {
        public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
    }
}
