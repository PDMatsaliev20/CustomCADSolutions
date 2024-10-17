using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface ICadQueries
{
    IQueryable<Cad> GetAll(bool asNoTracking = false);
    Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false);
    Task<bool> ExistsByIdAsync(int id);
    int Count(Func<Cad, bool> predicate);
}
