using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CadQueryRepository(CadContext context) : IQueryRepository<Cad>
    {
        public IQueryable<Cad> GetAll()
        {
            return context.Cads;
        }

        public async Task<Cad?> GetByIdAsync(params object[] id)
        {
            return await context.Cads.FindAsync(id).ConfigureAwait(false);
        }

        public int Count(Func<Cad, bool> predicate)
        {
            return context.Cads.Count(predicate);
        }
    }
}
