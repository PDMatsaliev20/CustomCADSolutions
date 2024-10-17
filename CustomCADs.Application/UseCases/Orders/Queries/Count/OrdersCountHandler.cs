using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.Count;

public class OrdersCountHandler(IOrderQueries queries) : IRequestHandler<OrdersCountQuery, int>
{
    public Task<int> Handle(OrdersCountQuery request, CancellationToken cancellationToken)
    {
        var response = queries.Count(order => request.Predicate(order.Adapt<OrderModel>()));

        return Task.FromResult(response);
    }
}
