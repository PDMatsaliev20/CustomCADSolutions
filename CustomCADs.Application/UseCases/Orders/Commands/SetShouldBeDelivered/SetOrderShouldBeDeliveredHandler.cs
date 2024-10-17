using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetShouldBeDelivered;

public class SetOrderShouldBeDeliveredHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetOrderShouldBeDeliveredCommand>
{
    public async Task Handle(SetOrderShouldBeDeliveredCommand req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, ct: ct)
            ?? throw new OrderNotFoundException(req.Id);

        order.ShouldBeDelivered = req.ShouldBeDelivered;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
