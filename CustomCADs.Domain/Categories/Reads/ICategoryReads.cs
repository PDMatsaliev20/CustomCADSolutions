namespace CustomCADs.Domain.Categories.Reads;

public interface ICategoryReads
{
    IQueryable<Category> GetAll(bool asNoTracking = false);
    Task<Category?> GetByIdAsync(int id, bool asNoTracking = false, CancellationToken ct = default);
    Task<bool> ExistsByIdAsync(int id, CancellationToken ct = default);
    int Count(Func<Category, bool> predicate);
}
