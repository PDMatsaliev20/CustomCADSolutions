using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.Count;

public class OrdersCountHandler(IOrderQueries queries) : IRequestHandler<OrdersCountQuery, int>
{
    public Task<int> Handle(OrdersCountQuery req, CancellationToken ct)
    {
        var response = queries.Count(order => req.Predicate(order.Adapt<OrderModel>()));

        return Task.FromResult(response);
    }
}
