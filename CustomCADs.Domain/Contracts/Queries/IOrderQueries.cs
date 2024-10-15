using CustomCADs.Domain.Entities;

namespace CustomCADs.Domain.Contracts.Queries;

public interface IOrderQueries
{
    IQueryable<Order> GetAll(bool asNoTracking = false);
    Task<Order?> GetByIdAsync(int id, bool asNoTracking = false);
    Task<bool> ExistsByIdAsync(int id);
    Task<int> CountAsync(Func<Order, bool> predicate, bool asNoTracking = false);
}
