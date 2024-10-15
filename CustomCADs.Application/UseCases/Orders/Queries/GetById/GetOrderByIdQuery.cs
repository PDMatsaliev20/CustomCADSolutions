using CustomCADs.Application.Models.Orders;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetById
{
    public record GetOrderByIdQuery(int Id) : IRequest<OrderModel> { }
}
