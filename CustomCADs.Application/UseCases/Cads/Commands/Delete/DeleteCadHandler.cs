using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Delete
{
    public class DeleteCadHandler(
        ICadQueries queries,
        IOrderQueries orderQueries,
        ICommands<Cad> commands,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteCadCommand>
    {
        public async Task Handle(DeleteCadCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> ordersWithCad = [.. orderQueries.GetAll(true).Filter(cadId: request.Id)];
            foreach (Order order in ordersWithCad)
            {
                order.Status = OrderStatus.Pending;
                order.DesignerId = null;
                order.Designer = null;
                order.CadId = null;
                order.Cad = null;
            }

            Cad cad = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
                ?? throw new OrderNotFoundException(request.Id);

            commands.Delete(cad);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
