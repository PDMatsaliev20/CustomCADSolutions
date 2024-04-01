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
        private readonly MapperConfiguration config = new(cfg =>
        {
            cfg.AddProfile<CadProfile>();
            cfg.AddProfile<OrderProfile>();
        });
        private IMapper Mapper { get => config.CreateMapper(); }

        public OrderService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<(string, int)> CreateAsync(OrderModel model)
        {
            if ((model.Cad == null && model.CadId == 0) || (model.Buyer == null && model.BuyerId == null))
            {
                throw new NullReferenceException();
            }

            Order order = Mapper.Map<Order>(model);
            Cad? cad = await repository.GetByIdAsync<Cad>(model.CadId);
            if (cad != null)
            {
                order.Cad = cad;
            }
            else order.Cad = Mapper.Map<Cad>(model.Cad);

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
                Order order = Mapper.Map<Order>(model);
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
                .FirstOrDefault(order => order.CadId == cadId
                                      && order.BuyerId == buyerId)
                ?? throw new KeyNotFoundException();

            repository.Delete(order);
            Cad? cad = await repository.GetByIdAsync<Cad>(cadId);
            if (cad != null)
            {
                repository.Delete<Cad>(cad);
            }

            await repository.SaveChangesAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int cadId, string buyerId)
        {
            Order order = await repository
                .GetByIdAsync<Order>(cadId, buyerId)
                ?? throw new KeyNotFoundException();

            OrderModel model = Mapper.Map<OrderModel>(order);
            return model;
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return Mapper.Map<OrderModel[]>(await repository.All<Order>().ToArrayAsync());
        }
    }
}
