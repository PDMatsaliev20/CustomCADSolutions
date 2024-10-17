using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface ICategoryQueries
{
    IQueryable<Category> GetAll(bool asNoTracking = false);
    Task<Category?> GetByIdAsync(int id, bool asNoTracking = false);
    Task<bool> ExistsByIdAsync(int id);
    int Count(Func<Category, bool> predicate);
}
