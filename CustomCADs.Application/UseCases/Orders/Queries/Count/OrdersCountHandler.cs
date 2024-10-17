using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Orders.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.Count;

public class OrdersCountHandler(IOrderReads reads) : IRequestHandler<OrdersCountQuery, int>
{
    public Task<int> Handle(OrdersCountQuery req, CancellationToken ct)
    {
        var response = reads.Count(order => req.Predicate(order.Adapt<OrderModel>()));

        return Task.FromResult(response);
    }
}
