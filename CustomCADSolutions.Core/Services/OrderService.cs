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

        public async Task<int> CreateAsync(OrderModel entity)
        {
            if (entity.Cad == null || entity.Buyer == null)
            {
                throw new NullReferenceException();
            }

            Cad cad = new()
            {
                Name = entity.Cad.Name,
                Category = entity.Cad.Category,
            };

            User buyer = repository
                .All<User>()
                .First(u => u.Id == entity.Buyer.Id)
                ?? new User { Username = entity.Buyer.Username, };

            Order order = new()
            {
                Cad = cad,
                Buyer = buyer,
                Description = entity.Description,
                OrderDate = entity.OrderDate,
            };

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

        public async Task EditAsync(OrderModel entity)
        {
            Order order = repository
                .All<Order>()
                .FirstOrDefault(order => order.CadId == entity.CadId
                                      && order.BuyerId == entity.BuyerId)
                ?? throw new NullReferenceException();


            order.Description = entity.Description;
            order.Cad.Name = entity.Cad.Name;
            order.Cad.Url = entity.Cad.Url;
            order.Cad.Category = entity.Cad.Category;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAllAsync()
        {
            return await repository
                .All<Order>()
                .Select(o => new OrderModel
                {
                    Id = o.Id,
                    CadId = o.CadId,
                    BuyerId = o.BuyerId,
                    Description = o.Description,
                    OrderDate = o.OrderDate,
                    Cad = new CadModel
                    {
                        Id = o.CadId,
                        Name = o.Cad.Name,
                        Category = o.Cad.Category,
                        Url = o.Cad.Url,
                        CreationDate = o.Cad.CreationDate,
                    },
                    Buyer = new UserModel
                    {
                        Id = o.BuyerId,
                        Username = o.Buyer.Username,
                    },
                })
                .ToArrayAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int id)
        {
            Order? order = await repository
                .All<Order>()
                .FirstOrDefaultAsync(o => o.Id == id)
                ?? throw new KeyNotFoundException();

            OrderModel model = ConvertEntityToModel(order);

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
                    Orders = u.Orders
                        .Select(o => new OrderModel
                        {
                            Id = o.Id,
                            CadId = o.CadId,
                            Description = o.Description,
                            OrderDate = o.OrderDate,
                            Cad = repository
                                .All<Cad>()
                                .Select(c => new CadModel() 
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    Category = c.Category, 
                                })
                                .First(c => c.Id == o.CadId)
                        })
                        .ToArray()
                }).ToArray();
        }

        private OrderModel ConvertEntityToModel(Order order)
        {
            return new()
            {
                Id = order.Id,
                CadId = order.CadId,
                BuyerId = order.BuyerId,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Cad = new CadModel
                {
                    Id = order.CadId,
                    Name = order.Cad.Name,
                    Category = order.Cad.Category,
                    Url = order.Cad.Url,
                    CreationDate = order.Cad.CreationDate,
                },
                Buyer = new UserModel
                {
                    Id = order.BuyerId,
                    Username = order.Buyer.Username,
                    Orders = order.Buyer.Orders.Select(o => new OrderModel()
                    {
                        Id = o.Id,
                        CadId = o.CadId,
                        BuyerId = o.BuyerId,
                        Description = o.Description,
                        OrderDate = o.OrderDate,
                        Cad = new CadModel
                        {
                            Id = o.CadId,
                            Name = o.Cad.Name,
                            Category = o.Cad.Category,
                            Url = o.Cad.Url,
                            CreationDate = o.Cad.CreationDate,
                        },
                    })
                    .ToArray()
                }
            };
        }
    }
}
