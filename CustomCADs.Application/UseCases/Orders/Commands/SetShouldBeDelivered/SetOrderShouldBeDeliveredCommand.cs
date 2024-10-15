using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetShouldBeDelivered
{
    public record SetOrderShouldBeDeliveredCommand(int Id, bool ShouldBeDelivered) : IRequest;
}
