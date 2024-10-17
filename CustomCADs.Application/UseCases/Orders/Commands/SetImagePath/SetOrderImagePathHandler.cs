using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetImagePath;

public class SetOrderImagePathHandler(
    IOrderQueries queries,
    IUnitOfWork unitOfWork) : IRequestHandler<SetOrderImagePathCommand>
{
    public async Task Handle(SetOrderImagePathCommand req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        order.ImagePath = req.ImagePath;
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
