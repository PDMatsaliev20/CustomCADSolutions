using AutoMapper;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Scaffolding;

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
                cfg.AddProfile<CadProfile>();
                cfg.AddProfile<OrderProfile>();
            }).CreateMapper();
        }
        
        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return mapper.Map<OrderModel[]>(await repository.All<Order>().ToArrayAsync());
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

        public async Task<int> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);

            EntityEntry<Order> entry = await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task CreateRangeAsync(params OrderModel[] models)
        {
            List<Order> orders = new();
            foreach (OrderModel model in models)
            {
                if (model.Buyer == null)
                {
                    throw new ArgumentException();
                }
                Order order = mapper.Map<Order>(model);
                orders.Add(order);
            }
            await repository.AddRangeAsync<Order>(orders.ToArray());
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(OrderModel model)
        {
            Order order = await repository.GetByIdAsync<Order>(model.Id)
                ?? throw new KeyNotFoundException();

            order.Description = model.Description;
            order.Status = model.Status;
            order.ShouldShow = model.ShouldShow;
            order.CadId = model.CadId;
            order.Cad.Name = model.Cad.Name;
            order.Cad.Category = model.Cad.Category;
            order.Cad.CreationDate = model.Cad.CreationDate;
            order.Cad.CreatorId = model.Cad.CreatorId;
            order.Cad.IsValidated = model.Cad.IsValidated;
            order.Cad.Bytes = model.Cad.Bytes;

            await repository.SaveChangesAsync();
        }
        
        public async Task FinishOrderAsync(int id, CadModel model)
        {
            Order order = await repository.GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Finished;
            
            order.Cad.Name = model.Name;
            order.Cad.Bytes = model.Bytes;
            order.Cad.IsValidated = model.IsValidated;
            order.Cad.CreationDate = model.CreationDate;
            order.Cad.CreatorId = model.CreatorId;
            order.Cad.CategoryId = model.CategoryId;

            await repository.SaveChangesAsync();
        }

        public async Task EditRangeAsync(params OrderModel[] models)
        {
            foreach (OrderModel model in models)
            {
                Order order = await repository.GetByIdAsync<Order>(model.Id)
                    ?? throw new KeyNotFoundException();

                order.Description = model.Description;
                order.Status = model.Status;
                order.ShouldShow = model.ShouldShow;
                order.Cad.Name = model.Cad.Name;
                order.Cad.Category = model.Cad.Category;
                order.Cad.CreationDate = model.Cad.CreationDate;
                order.Cad.CreatorId = model.Cad.CreatorId;
                order.Cad.IsValidated = model.Cad.IsValidated;
            }
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
