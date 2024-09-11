using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence.Repositories.Orders
{
    public class OrderQueries(ApplicationContext context) : IQueries<Order, int>
    {
        public async Task<IEnumerable<Order>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Order, bool>? customFilter = null, bool asNoTracking = false)
        {
            IQueryable<Order> query = context.Orders
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery();

            if (user != null)
            {
                query = query.Where(c => c.Buyer.UserName == user);
            }
            if (status != null && Enum.TryParse(status, ignoreCase: true, out OrderStatus orderStatus))
            {
                query = query.Where(c => c.Status == orderStatus);
            }

            if (customFilter != null)
            {
                query = query.Where(o => customFilter(o));
            }

            if (category != null)
            {
                query = query.Where(c => c.Category.Name == category);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(owner))
            {
                query = query.Where(c => c.Buyer.UserName!.Contains(owner));
            }
            query = sorting.ToLower() switch
            {
                "newest" => query.OrderByDescending(c => c.OrderDate),
                "oldest" => query.OrderBy(c => c.OrderDate),
                "alphabetical" => query.OrderBy(c => c.Name),
                "unalphabetical" => query.OrderByDescending(c => c.Name),
                "category" => query.OrderBy(m => m.Category.Name),
                _ => query.OrderByDescending(c => c.Id),
            };

            Order[] orders = await query
                .ToArrayAsync()
                .ConfigureAwait(false);

            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            Order? order = await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            return order;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await context.Orders
                .AnyAsync(o => o.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Func<Order, bool> predicate, bool asNoTracking = false)
        {
            return await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Buyer)
                .CountAsync(c => predicate(c))
                .ConfigureAwait(false);
        }
    }
}
