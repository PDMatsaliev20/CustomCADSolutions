using CustomCADs.Domain.Orders;

namespace CustomCADs.Domain.Orders.Queries;

public interface IOrderQueries
{
    IQueryable<Order> GetAll(bool asNoTracking = false);
    Task<Order?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    int Count(Func<Order, bool> predicate);
}
