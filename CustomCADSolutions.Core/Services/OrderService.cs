using AutoMapper;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public OrderService(IRepository repository)
        {
            this.repository = repository;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CadCoreProfile>();
                cfg.AddProfile<OrderCoreProfile>();
            }).CreateMapper();
        }
        
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
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                return validationResults.Select(result => result.ErrorMessage ?? string.Empty).ToList();
            }

            return new List<string>();
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
            order.Status = model.Status;
            order.ShouldShow = model.ShouldShow;
            order.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync();
        }
        
        public async Task FinishOrderAsync(int id, OrderModel model)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Finished;

            CadModel cad = model.Cad!;
            order.Cad = new()
            {
                Name = cad.Name,
                Price = cad.Price,
                CategoryId = cad.CategoryId,
                Bytes = cad.Bytes,
                IsValidated = cad.IsValidated,
                CreatorId = cad.CreatorId,
                CreationDate = cad.CreationDate,
            };
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
