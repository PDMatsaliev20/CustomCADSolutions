using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace CustomCADs.Core.Services
{
    public class OrderService(IRepository repository) : IOrderService
    {
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CategoryCoreProfile>();
                cfg.AddProfile<CadCoreProfile>();
                cfg.AddProfile<OrderCoreProfile>();
            }).CreateMapper();

        public async Task<OrderResult> GetAllAsync(OrderQuery query, OrderSearch search, OrderPagination pagination, Expression<Func<Order, bool>>? customFilter = null)
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

            // Optional custom
            if (customFilter != null)
            {
                dbOrders = dbOrders.Where(customFilter);
            }

            // Search & Sort
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                dbOrders = dbOrders.Where(o => o.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Buyer))
            {
                dbOrders = dbOrders.Where(o => o.Buyer.UserName!.Contains(search.Buyer));
            }
            if (!string.IsNullOrWhiteSpace(search.Category))
            {
                dbOrders = dbOrders.Where(o => o.Category.Name == search.Category);
            }
            dbOrders = search.Sorting.ToLower() switch
            {
                "newest" => dbOrders.OrderBy(o => o.OrderDate),
                "oldest" => dbOrders.OrderByDescending(o => o.OrderDate),
                "alphabetical" => dbOrders.OrderBy(o => o.Name),
                "unalphabetical" => dbOrders.OrderByDescending(o => o.Name),
                "category" => dbOrders.OrderBy(o => o.Category.Name),
                _ => dbOrders.OrderBy(o => o.Id)
            };

            // Pagination
            Order[] orders = await dbOrders
                .Skip((pagination.CurrentPage - 1) * pagination.CadsPerPage)
                .Take(pagination.CadsPerPage)
                .ToArrayAsync();

            OrderModel[] models = mapper.Map<OrderModel[]>(orders);
            return new()
            {
                Count = dbOrders.Count(),
                Orders = models
            };
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            OrderModel model = mapper.Map<OrderModel>(order);
            return model;
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await repository.GetByIdAsync<Order>(id) != null;

        public IList<string> ValidateEntity(OrderModel model)
        {
            List<ValidationResult> validationResults = [];
            if (!Validator.TryValidateObject(model, new(model), validationResults, true))
            {
                return validationResults.Select(result => result.ErrorMessage ?? string.Empty).ToList();
            }

            return [];
        }

        public async Task<int> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);

            EntityEntry<Order> entry = await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task EditAsync(int id, OrderModel model)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Name = model.Name;
            order.Description = model.Description;
            order.ShouldBeDelivered = model.ShouldBeDelivered;
            order.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync();
        }

        public async Task BeginAsync(int id, string designerId)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Begun;
            order.DesignerId = designerId;

            await repository.SaveChangesAsync();
        }
        
        public async Task ReportAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Reported;
            await repository.SaveChangesAsync();
        }
        
        public async Task CancelAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.DesignerId = null;
            order.Status = OrderStatus.Pending;

            await repository.SaveChangesAsync();
        }
        
        public async Task CompleteAsync(int id, int cadId)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.CadId = cadId;
            order.Status = OrderStatus.Finished;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            repository.Delete(order);
            await repository.SaveChangesAsync();
        }
    }
}
