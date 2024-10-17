using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Cads;

public class CadQueries(ApplicationContext context) : ICadQueries
{
    public IQueryable<Cad> GetAll(bool asNoTracking = false)
        => context.Cads
            .Query(asNoTracking)
            .Include(o => o.Category)
            .Include(o => o.Creator)
            .AsSplitQuery();

    public async Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default)
        => await context.Cads
            .Query(asNoTracking)
            .Include(o => o.Category)
            .Include(o => o.Creator)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id, ct)
            .ConfigureAwait(false);

    public async Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default)
        => await context.Cads
            .AnyAsync(o => o.Id == id, ct)
            .ConfigureAwait(false);

    public int Count(Func<Cad, bool> predicate)
        => context.Cads
            .AsNoTracking()
            .Include(c => c.Creator)
            .Count(predicate);
}
