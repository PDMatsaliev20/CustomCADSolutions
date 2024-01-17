using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Net.Http.Headers;

namespace CustomCADSolutions.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository repository;

        public OrderService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task CreateAsync(OrderModel entity)
        {
            if (entity.Cad == null)
            {
                throw new ArgumentNullException();
            }

            Cad cad = new()
            {
                Name = entity.Cad.Name,
                Category = entity.Cad.Category,
                Url = String.Empty,
                CreationDate = null,
            };

            Order order = new()
            {
                Cad = cad,
                Buyer = new User { Username = entity.Buyer.Username },
                Description = entity.Description,
                OrderDate = entity.OrderDate,
            };
            
            await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Order? order = await repository.GetByIdAsync<Order>(id);
            if (order != null)
            {
                repository.Delete<Order>(order);
            }
        }

        public async Task EditAsync(OrderModel entity)
        {
            Order order = repository
                .All<Order>()
                .FirstOrDefault(order => order.CadId == entity.CadId
                                      && order.BuyerId == entity.BuyerId)
                ?? throw new NullReferenceException();

            order.OrderDate = entity.OrderDate;
            order.Description = entity.Description;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await repository
                .All<Order>()
                .Select(o => new OrderModel
                {
                    CadId = o.CadId,
                    BuyerId = o.BuyerId,
                    Description = o.Description,
                    OrderDate = o.OrderDate,
                })
                .ToArrayAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order? order = await repository
                .GetByIdAsync<Order>(id)
                ?? throw new NullReferenceException();

            OrderModel model = new()
            {
                CadId = order.CadId,
                BuyerId = order.BuyerId,
                Description = order.Description,
                OrderDate = order.OrderDate,
            };

            return model;
        }
        public IEnumerable<UserModel> GetAllUsers()
        {
            return repository
                .All<User>()
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Orders = repository
                        .All<Order>()
                        .Where(o => o.BuyerId == u.Id)
                        .Select(o => new OrderModel
                        {
                            CadId = o.CadId,
                            Description = o.Description,
                            OrderDate = o.OrderDate,
                        })
                        .ToArray()
                }).ToArray();
        }
    }
}
