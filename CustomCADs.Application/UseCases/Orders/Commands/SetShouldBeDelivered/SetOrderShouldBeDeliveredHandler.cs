using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
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
