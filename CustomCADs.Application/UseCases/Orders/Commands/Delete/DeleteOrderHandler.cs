using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Delete;

public class DeleteOrderHandler(
    IOrderReads reads, 
    IWrites<Order> writes, 
    IUnitOfWork uow) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand req, CancellationToken ct)
    {
        Order order = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        writes.Delete(order);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
