using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetImagePath;

public class SetOrderImagePathHandler(
    IOrderReads reads,
    IUnitOfWork uow) : IRequestHandler<SetOrderImagePathCommand>
{
    public async Task Handle(SetOrderImagePathCommand req, CancellationToken ct)
    {
        Order order = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        order.ImagePath = req.ImagePath;
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
