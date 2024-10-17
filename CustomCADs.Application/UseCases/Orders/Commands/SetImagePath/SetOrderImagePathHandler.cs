using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
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
