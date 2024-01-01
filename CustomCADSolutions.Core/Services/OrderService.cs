using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
            Order order = new()
            {
                CADId = entity.CADId,
                BuyerId = entity.BuyerId,
                OrderDate = entity.OrderDate,
                CAD = entity.CAD,
                Buyer = entity.Buyer
            };

            await repository.AddAsync<Order>(order);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int cadId, int buyerId)
        {
            Order? order = await repository.GetByIdAsync<Order>(cadId, buyerId);

            if (order != null)
            {
                repository.Delete<Order>(order);
            }
        }

        public async Task EditAsync(OrderModel entity)
        {
            Order? order = await repository
                .All<Order>()
                .FirstOrDefaultAsync(order
                    => order.CADId == entity.CADId && order.BuyerId == entity.BuyerId);

            if (order == null)
            {
                throw new ArgumentException($"{entity.CAD.Name} has not been bought by {entity.Buyer}!");
            }

            order.CADId = entity.CADId;
            order.BuyerId = entity.BuyerId;
            order.OrderDate = entity.OrderDate;
            order.CAD = entity.CAD;
            order.Buyer = entity.Buyer;

            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderModel>> GetAll()
        {
            return await repository
                .All<Order>()
                .Select(order => new OrderModel
                {
                    CADId = order.CADId,
                    BuyerId = order.BuyerId,
                    OrderDate = order.OrderDate,
                    CAD = order.CAD,
                    Buyer = order.Buyer
                })
                .ToListAsync();
        }

        public async Task<OrderModel> GetByIdAsync(int cadId, int buyerId)
        {
            Order order = await repository
                .All<Order>()
                .FirstOrDefaultAsync(order => order.CADId == cadId && order.BuyerId == buyerId)
                ?? throw new ArgumentException("Model doesn't exist");

            OrderModel model = new()
            {
                CADId = order.CADId,
                BuyerId = order.BuyerId,
                OrderDate = order.OrderDate,
                CAD = order.CAD,
                Buyer = order.Buyer
            };

            return model;
        }


        public async Task<IEnumerable<CADModel>> GetCADsAsync()
        {
            List<CADModel> cads = await repository
                .All<CAD>()
                .Select(cad => new CADModel
                {
                    Id = cad.Id,
                    Name = cad.Name,
                    Description = cad.Description,
                    CreationDate = cad.CreationDate,
                    Orders = cad.Orders,
                })
                .ToListAsync();

            return cads;
        }
    }
}
