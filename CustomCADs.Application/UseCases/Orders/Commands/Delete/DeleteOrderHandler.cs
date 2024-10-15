using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Delete
{
    public class DeleteOrderHandler(
        IOrderQueries queries, 
        ICommands<Order> commands, 
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand>
    {
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(request.Id);

            commands.Delete(order);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
