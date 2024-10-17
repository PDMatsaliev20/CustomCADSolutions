using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface ICadQueries
{
    IQueryable<Cad> GetAll(bool asNoTracking = false);
    Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    int Count(Func<Cad, bool> predicate);
}
