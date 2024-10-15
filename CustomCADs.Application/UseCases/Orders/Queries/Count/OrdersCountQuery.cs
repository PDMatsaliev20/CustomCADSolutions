using CustomCADs.Application.Models.Orders;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.Count
{
    public record OrdersCountQuery(Func<OrderModel, bool> Predicate) : IRequest<int> { }
}
