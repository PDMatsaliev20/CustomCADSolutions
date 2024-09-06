using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using CustomCADs.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class OrderQueryRepository(CadContext context, IMapper mapper) : IQueryRepository<Order, int>
    {
        public async Task<IEnumerable<Order>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Order, bool>? customFilter = null, bool asNoTracking = false)
        {
            IQueryable<POrder> query = context.Orders
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
                query = query.Where(c => customFilter(mapper.Map<Order>(c)));
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

            POrder[] orders = await query.ToArrayAsync().ConfigureAwait(false);
            return orders.Select(mapper.Map<Order>);
        }

        public async Task<Order?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            POrder? order = await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Buyer)
                .Include(o => o.Designer)
                .FirstOrDefaultAsync(o => o.Id == id)
                .ConfigureAwait(false);

            return mapper.Map<Order?>(order); 
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Orders.AnyAsync(o => o.Id == id).ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Order, bool> predicate, bool asNoTracking = false)
        {
            POrder[] entities = await context.Orders
                .Query(asNoTracking)
                .Include(o => o.Buyer)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return entities.Count(o => predicate(mapper.Map<Order>(o)));
        }
    }
}
