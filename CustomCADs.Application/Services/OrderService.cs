using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Services
{
    public class OrderService(IDbTracker dbTracker,
        IQueryRepository<Order, int> queries,
        ICommandRepository<Order> commands, 
        IMapper mapper) : IOrderService
    {
        public async Task<OrderResult> GetAllAsync(OrderQuery query, SearchModel search, PaginationModel pagination, Func<Order, bool>? customFilter = null)
        {
            IEnumerable<Order> orders = await queries.GetAll(
                user: query.Buyer,
                status: query.Status.ToString(),
                name: search.Name,
                owner: search.Owner,
                category: search.Category,
                sorting: search.Sorting,
                customFilter: customFilter,
                asNoTracking: true
            ).ConfigureAwait(false);

            OrderModel[] models = mapper.Map<OrderModel[]>(orders
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
            );

            return new()
            {
                Count = orders.Count(),
                Orders = models
            };
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order order = await queries.GetByIdAsync(id, asNoTracking: true)
                .ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            OrderModel model = mapper.Map<OrderModel>(order);
            return model;
        }
        
        public async Task<CadModel> GetCadAsync(int id)
        {
            Order? order = await queries.GetByIdAsync(id, asNoTracking: true)
                .ConfigureAwait(false);

            if (order == null || order.CadId == null)
            {
                throw new KeyNotFoundException();
            }

            CadModel model = mapper.Map<CadModel>(order.Cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await queries.ExistsByIdAsync(id).ConfigureAwait(false);

        public async Task<int> Count(Func<OrderModel, bool> predicate)
        {
            return await queries.CountAsync(
                order => predicate(mapper.Map<OrderModel>(order)),
                asNoTracking: true
            ).ConfigureAwait(false);
        }
        
        public async Task<bool> HasCadAsync(int id)
        {
            Order? order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();
            return order.CadId != null;
        }
        
        public async Task<bool> CheckOwnership(int id, string username)
        {
            Order? order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();
            return order.Buyer.UserName == username;
        }

        public async Task<int> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);

            await commands.AddAsync(order).ConfigureAwait(false);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);

            return order.Id;
        }

        public async Task EditAsync(int id, OrderModel model)
        {
            Order order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Name = model.Name;
            order.Description = model.Description;
            order.ShouldBeDelivered = model.ShouldBeDelivered;
            order.CategoryId = model.CategoryId;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await queries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            commands.Delete(order);
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
