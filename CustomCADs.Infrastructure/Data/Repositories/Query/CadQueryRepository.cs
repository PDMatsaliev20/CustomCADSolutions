using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CadQueryRepository(CadContext context) : IQueryRepository<Cad>
    {
        public IQueryable<Cad> GetAll()
        {
            return context.Cads
                .Include(c => c.Category)
                .Include(c => c.Creator);
        }

        public async Task<Cad?> GetByIdAsync(params object[] id)
        {
            return await context.Cads
                .Include(c => c.Category)
                .Include(c => c.Creator)
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id.Equals(id[0]))
                .ConfigureAwait(false);
        }

        public int Count(Func<Cad, bool> predicate)
        {
            return context.Cads
                .Include(c => c.Creator)
                .Count(predicate);
        }
    }
}
