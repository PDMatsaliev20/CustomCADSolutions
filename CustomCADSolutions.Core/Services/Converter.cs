﻿using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;
using System.ComponentModel;

namespace CustomCADSolutions.Core.Services
{
    public class Converter : IConverter
    {
        public CadModel CadToModel(Cad cad, bool firstTime = true)
        {
            CadModel model = new()
            {
                Id = cad.Id,
                Name = cad.Name,
                Category = cad.Category,
                CreationDate = cad.CreationDate,
                CreatorId = cad.CreatorId,
                Creator = cad.Creator,
                Coords = (cad.X, cad.Y, cad.Z),
            };

            if (firstTime && cad.Orders.Any())
            {
                model.Orders = cad.Orders
                    .Select(o => OrderToModel(o, false))
                    .ToArray();
            }

            return model;
        }

        public Cad ModelToCad(CadModel model, bool firstTime = true)
        {
            Cad cad = new()
            {
                Name = model.Name,
                Category = model.Category,
                CreationDate = DateTime.Now,
                CreatorId = model.CreatorId,
                X = model.Coords.Item1,
                Y = model.Coords.Item2,
                Z = model.Coords.Item3,
            };

            if (firstTime && model.Orders.Any())
            {
                cad.Orders = model.Orders
                    .Select(o => ModelToOrder(o, false))
                    .ToArray();
            }

            return cad;
        }

        public OrderModel OrderToModel(Order order, bool firstTime = true)
        {
            return new()
            {
                CadId = order.CadId,
                BuyerId = order.BuyerId,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Buyer = order.Buyer,
                Cad = firstTime ? CadToModel(order.Cad) : null!,
            };
        }

        public Order ModelToOrder(OrderModel model, bool fisrTime = true)
        {
            Order order = new()
            {
                Description = model.Description,
                OrderDate = model.OrderDate,
                Buyer = model.Buyer,
                Cad = fisrTime ? ModelToCad(model.Cad) : null!,
            };
            return order;
        }
    }
}
