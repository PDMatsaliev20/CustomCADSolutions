using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetShouldBeDelivered;

public class SetOrderShouldBeDeliveredHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetOrderShouldBeDeliveredCommand>
{
    public async Task Handle(SetOrderShouldBeDeliveredCommand request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id)
            ?? throw new OrderNotFoundException(request.Id);

        order.ShouldBeDelivered = request.ShouldBeDelivered;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
