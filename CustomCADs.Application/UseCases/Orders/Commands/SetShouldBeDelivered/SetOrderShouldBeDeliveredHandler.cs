using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetShouldBeDelivered;

public class SetOrderShouldBeDeliveredHandler(IOrderReads reads, IUnitOfWork uow) : IRequestHandler<SetOrderShouldBeDeliveredCommand>
{
    public async Task Handle(SetOrderShouldBeDeliveredCommand req, CancellationToken ct)
    {
        Order order = await reads.GetByIdAsync(req.Id, ct: ct)
            ?? throw new OrderNotFoundException(req.Id);

        order.ShouldBeDelivered = req.ShouldBeDelivered;

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
