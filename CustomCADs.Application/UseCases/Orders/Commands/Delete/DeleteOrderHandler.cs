using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using CustomCADs.Domain.Shared;
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
