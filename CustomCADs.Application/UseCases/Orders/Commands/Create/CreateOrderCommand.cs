using CustomCADs.Application.Models.Orders;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Create
{
    public record CreateOrderCommand(OrderModel Model) : IRequest<int> { }
}
