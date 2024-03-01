using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Core.Services
{
    public class Converter : IConverter
    {
        public CadModel CadToModel(Cad cad, bool firstTime = true)
            => new()
            {
                Id = cad.Id,
                Name = cad.Name,
                Validated = cad.Validated,
                CreationDate = cad.CreationDate,
                Coords = (cad.X, cad.Y, cad.Z),
                SpinAxis = cad.SpinAxis,
                SpinFactor = cad.SpinFactor,
                CategoryId = cad.CategoryId,
                CreatorId = cad.CreatorId,
                Creator = cad.Creator,
                Category = cad.Category,
                Orders = firstTime && cad.Orders.Any() ?
                    cad.Orders.Select(o => OrderToModel(o, false)).ToList()
                    : new List<OrderModel>()
            };

        public OrderModel OrderToModel(Order order, bool firstTime = true)
            => new()
            {
                CadId = order.CadId,
                BuyerId = order.BuyerId,
                Description = order.Description,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShouldShow = order.ShouldShow,
                Buyer = order.Buyer,
                Cad = firstTime ? CadToModel(order.Cad) : null!,
            };

        public Order ModelToOrder(OrderModel model, bool fisrTime = true)
            => new()
            {
                Description = model.Description,
                OrderDate = model.OrderDate,
                Status = model.Status,
                ShouldShow = model.ShouldShow,
                Buyer = model.Buyer,
                Cad = fisrTime ? ModelToCad(model.Cad) : null!,
            };

        public Cad ModelToCad(CadModel model, bool firstTime = true)
            => new()
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                Validated = model.Validated,
                CreationDate = model.CreationDate ?? DateTime.Now,
                CreatorId = model.CreatorId,
                X = model.Coords.Item1,
                Y = model.Coords.Item2,
                Z = model.Coords.Item3,
                SpinAxis = model.SpinAxis,
                SpinFactor = model.SpinFactor,
                Orders = firstTime && model.Orders.Any() ?
                    model.Orders.Select(o => ModelToOrder(o, false)).ToList()
                    : new List<Order>()
            };


    }
}
