using AutoMapper;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<(string, int)> CreateAsync(OrderModel model)
        {
            Order order = mapper.Map<Order>(model);
            order.Cad = await repository.GetByIdAsync<Cad>(model.CadId)
                ?? mapper.Map<Cad>(model.Cad);

            EntityEntry<Order> entry = await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();

            return (entry.Entity.BuyerId, entry.Entity.CadId);
        }

        public async Task CreateRangeAsync(params OrderModel[] models)
        {
            List<Order> orders = new();
            foreach (OrderModel model in models)
            {
                if (model.Cad == null || model.Buyer == null)
                {
                    throw new NullReferenceException();
                }
                Order order = mapper.Map<Order>(model);
                orders.Add(order);
            }
            await repository.AddRangeAsync<Order>(orders.ToArray());
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(OrderModel model)
        {
            Order order = await repository.All<Order>()
                .FirstOrDefaultAsync(order => order.CadId == model.CadId && order.BuyerId == model.BuyerId)
                ?? throw new KeyNotFoundException();

            order.Description = model.Description;
            order.Status = model.Status;
            order.ShouldShow = model.ShouldShow;
            order.Cad.Name = model.Cad.Name;
            order.Cad.Category = model.Cad.Category;
            order.Cad.CreationDate = model.Cad.CreationDate;
            order.Cad.CreatorId = model.Cad.CreatorId;
            order.Cad.IsValidated = model.Cad.IsValidated;

            await repository.SaveChangesAsync();
        }

        public async Task EditRangeAsync(params OrderModel[] models)
        {
            foreach (OrderModel model in models)
            {
                Order order = await repository.All<Order>()
                    .FirstOrDefaultAsync(order => order.CadId == model.CadId && order.BuyerId == model.BuyerId)
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

        public async Task DeleteAsync(int cadId, string buyerId)
        {
            Order order = repository
                .All<Order>()
                .FirstOrDefault(order 
                    => order.CadId == cadId && order.BuyerId == buyerId)
                ?? throw new KeyNotFoundException();

            repository.Delete(order);

            await repository.SaveChangesAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int cadId, string buyerId)
        {
            Order order = await repository
                .GetByIdAsync<Order>(cadId, buyerId)
                ?? throw new KeyNotFoundException();

            OrderModel model = mapper.Map<OrderModel>(order);
            return model;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return mapper.Map<OrderModel[]>(await repository.All<Order>().ToArrayAsync());
        }

        public async Task<bool> ExistsByIdAsync(int cadId, string buyerId)
            => await repository.GetByIdAsync<Order>(cadId, buyerId) != null;
        
    }
}
