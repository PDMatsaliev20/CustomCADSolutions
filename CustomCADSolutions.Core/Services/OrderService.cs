using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;
        private readonly UserManager<IdentityUser> userManager;

        public OrderService(IRepository repository, UserManager<IdentityUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;

        }

        public async Task<int> CreateAsync(OrderModel model)
        {
            if (model.Cad == null || model.Buyer == null)
            {
                throw new NullReferenceException();
            }
            Order order = await ConvertModelToEntity(model);

            await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();

            return order.Id;
        }

        public async Task DeleteAsync(int id)
        {
            Order? order = await repository.GetByIdAsync<Order>(id);
            if (order != null)
            {
                repository.Delete<Order>(order);
            }
        }

        public async Task EditAsync(OrderModel model)
        {
            Order order = repository
                .All<Order>()
                .FirstOrDefault(order => order.CadId == model.CadId
                                      && order.BuyerId == model.BuyerId)
                ?? throw new NullReferenceException();


            order.Description = model.Description;
            order.Cad.Name = model.Cad.Name;
            order.Cad.Url = model.Cad.Url;
            order.Cad.Category = model.Cad.Category;
            order.Cad.CreationDate = model.Cad.CreationDate;

            await repository.SaveChangesAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order order = await repository
                .GetByIdAsync<Order>(id)
                ?? throw new KeyNotFoundException();

            OrderModel model = ConvertEntityToModel(order);

            return model;
        }
        
        public async Task<IEnumerable<OrderModel>> GetAllAsync() 
            => await repository.All<Order>()
                .Select(o => ConvertEntityToModel(o))
                .ToArrayAsync();

        private static OrderModel ConvertEntityToModel(Order order)
        {
            return new()
            {
                Id = order.Id,
                CadId = order.CadId,
                BuyerId = order.BuyerId,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Buyer = order.Buyer,
                Cad = new CadModel
                {
                    Id = order.CadId,
                    Name = order.Cad.Name,
                    Category = order.Cad.Category,
                    Url = order.Cad.Url,
                    CreationDate = order.Cad.CreationDate,
                },
            };
        }

        private async Task<Order> ConvertModelToEntity(OrderModel model)
        {
            return new()
            {
                Description = model.Description,
                OrderDate = model.OrderDate,
                Buyer = await userManager.FindByIdAsync(model.Buyer.Id),
                Cad = new()
                {
                    Name = model.Cad.Name,
                    Category = model.Cad.Category,
                },
            };
        }
    }
}
