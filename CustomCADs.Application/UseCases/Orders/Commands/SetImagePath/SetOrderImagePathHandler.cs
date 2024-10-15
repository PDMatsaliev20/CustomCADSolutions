using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetImagePath;

public class SetOrderImagePathHandler(
    IOrderQueries queries,
    IUnitOfWork unitOfWork) : IRequestHandler<SetOrderImagePathCommand>
{
    public async Task Handle(SetOrderImagePathCommand request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        order.ImagePath = request.ImagePath;
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
