using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Delete;

public class DeleteOrderHandler(
    IOrderQueries queries, 
    ICommands<Order> commands, 
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        commands.Delete(order);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
