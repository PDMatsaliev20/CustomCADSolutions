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

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            Order[] orders = await repository.All<Order>().ToArrayAsync();

            OrderModel[] models = mapper.Map<OrderModel[]>(orders);
            return models;
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

        public async Task EditStatusAsync(int id, OrderStatus status)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Status = status;
            await repository.SaveChangesAsync();
        }
        
        public async Task CompleteAsync(int id, int cadId)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.CadId = cadId;
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
