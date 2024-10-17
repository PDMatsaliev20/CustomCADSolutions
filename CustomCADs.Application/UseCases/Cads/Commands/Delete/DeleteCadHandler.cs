using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Common.Helpers;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Delete;

public class DeleteCadHandler(
    ICadQueries queries,
    IOrderQueries orderQueries,
    ICommands<Cad> commands,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCadCommand>
{
    public async Task Handle(DeleteCadCommand req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        ResetOrders(req.Id);
        commands.Delete(cad);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    private void ResetOrders(int id)
    {
        IEnumerable<Order> ordersWithCad = [ .. orderQueries.GetAll().Filter(cadId: id)];

        foreach (Order order in ordersWithCad)
        {
            order.Status = OrderStatus.Pending;
            order.DesignerId = null;
            order.Designer = null;
            order.CadId = null;
            order.Cad = null;
        }
    }
}
