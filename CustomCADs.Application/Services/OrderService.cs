using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CustomCADs.Application.Services
{
    public class OrderService(IRepository repository, IMapper mapper) : IOrderService
    {
        public async Task<OrderResult> GetAllAsync(OrderQuery query, SearchModel search, PaginationModel pagination, Expression<Func<Order, bool>>? customFilter = null)
        {
            IQueryable<Order> dbOrders = repository.All<Order>();

            // Querying
            if (!string.IsNullOrWhiteSpace(query.Buyer))
            {
                dbOrders = dbOrders.Where(o => o.Buyer.UserName == query.Buyer);
            }
            if (query.Status != null)
            {
                dbOrders = dbOrders.Where(o => o.Status == query.Status);
            }

            // Optional custom filter
            if (customFilter != null)
            {
                dbOrders = dbOrders.Where(customFilter);
            }

            // Search & Sort
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                dbOrders = dbOrders.Where(o => o.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Owner))
            {
                dbOrders = dbOrders.Where(o => o.Buyer.UserName!.Contains(search.Owner));
            }
            if (!string.IsNullOrWhiteSpace(search.Category))
            {
                dbOrders = dbOrders.Where(o => o.Category.Name == search.Category);
            }
            dbOrders = search.Sorting.ToLower() switch
            {
                "newest" => dbOrders.OrderByDescending(o => o.OrderDate),
                "oldest" => dbOrders.OrderBy(o => o.OrderDate),
                "alphabetical" => dbOrders.OrderBy(o => o.Name),
                "unalphabetical" => dbOrders.OrderByDescending(o => o.Name),
                "category" => dbOrders.OrderBy(o => o.Category.Name),
                _ => dbOrders.OrderByDescending(o => o.Id)
            };

            // Pagination
            Order[] orders = await dbOrders
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
                .ToArrayAsync()
                .ConfigureAwait(false);

            OrderModel[] models = mapper.Map<OrderModel[]>(orders);
            return new()
            {
                Count = dbOrders.Count(),
                Orders = models
            };
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            OrderModel model = mapper.Map<OrderModel>(order);
            return model;
        }
        
        public async Task<CadModel> GetCadAsync(int id)
        {
            Order? order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false);

            if (order == null || order.CadId == null)
            {
                throw new KeyNotFoundException();
            }

            CadModel model = mapper.Map<CadModel>(order.Cad);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Order>(id).ConfigureAwait(false) != null;
        public int Count(Func<OrderModel, bool> predicate)
            =>  repository.Count<Order>(cad => predicate(mapper.Map<OrderModel>(cad)));
        
        public async Task<bool> HasCadAsync(int id)
        {
            Order? order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();
            return order.CadId != null;
        }
        
        public async Task<bool> CheckOwnership(int id, string username)
        {
            Order? order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();
            return order.Buyer.UserName == username;
        }

        public async Task<int> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);

            EntityEntry<Order> entry = await repository.AddAsync(order).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task EditAsync(int id, OrderModel model)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Name = model.Name;
            order.Description = model.Description;
            order.ShouldBeDelivered = model.ShouldBeDelivered;
            order.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task BeginAsync(int id, string designerId)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Begun;
            order.DesignerId = designerId;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public async Task ReportAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Reported;
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public async Task CancelAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.DesignerId = null;
            order.Status = OrderStatus.Pending;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
        
        public async Task CompleteAsync(int id, int cadId)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.CadId = cadId;
            order.Status = OrderStatus.Finished;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            repository.Delete(order);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
