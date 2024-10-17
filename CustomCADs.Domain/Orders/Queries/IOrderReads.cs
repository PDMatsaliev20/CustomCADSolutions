namespace CustomCADs.Domain.Orders.Reads;

public interface IOrderReads
{
    IQueryable<Order> GetAll(bool asNoTracking = false);
    Task<Order?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    int Count(Func<Order, bool> predicate);
}
